import * as L from 'leaflet';

import IAnalytics from '../models/analytics';
import IInitialiseOptions from '../models/initializeOptions';
import IMapMarker from '../models/mapMarker';
import { getQueryString, updateQueryString } from '../utils/url';

import addPois from './addPois';
import getIconDimensions from './getIconDimensions';

const POPUP_MAX_WIDTH = 640;

const POPUP_CLOSE_DELAY = 25;

interface ILeafletPopup {
    _container: HTMLElement;
}

interface ILeafetMarker {
    _popup: ILeafletPopup;
}

interface IDisplayPopupsState {
    activePopup: L.Marker | null;
    activePopupMarker: IMapMarker | null;
}

const queryStringParams = {
    poi: 'poi',
};

const state: IDisplayPopupsState = {
    activePopup: null,
    activePopupMarker: null,
};

const displayPois = (map: L.Map, options: IInitialiseOptions): void => {
    if (!options.pois) {
        return;
    }

    const analytics: IAnalytics | null = options.analytics
        ? JSON.parse(options.analytics)
        : null;
    const pois = addPois(map, options, JSON.parse(options.pois));

    const fetchPoiContent = (poi: IMapMarker) => {
        if (!poi || !poi.icon) {
            return;
        }

        const dimensions = getIconDimensions(map, options, poi.icon);

        window
            .fetch(
                `${options.poiDisplayUrl}?contentItemId=${options.contentItemId}&poiContentItemId=${poi.contentItemId}`
            )
            .then((response) => response.json())
            .then((data) => {
                state.activePopup = poi.marker
                    .setIcon(
                        L.icon({
                            iconUrl: poi.icon?.path ?? '',

                            iconAnchor: [
                                dimensions.width / 2,
                                dimensions.height / 2,
                            ],
                            iconSize: [dimensions.width, dimensions.height],

                            popupAnchor: [0, 300],
                        })
                    )
                    .bindPopup(data.Content, {
                        autoPan: false,
                        maxWidth: POPUP_MAX_WIDTH,
                    })
                    .openPopup();

                const $activePoi = (
                    state.activePopup as unknown as ILeafetMarker
                )._popup._container;

                const ro = new window.ResizeObserver(() => {
                    if (!$activePoi || !poi || !poi.icon) {
                        return;
                    }

                    poi.marker.setIcon(
                        L.icon({
                            iconUrl: poi.icon.path,

                            iconAnchor: [
                                dimensions.width / 2,
                                dimensions.height / 2,
                            ],
                            iconSize: [dimensions.width, dimensions.height],

                            popupAnchor: [0, $activePoi.offsetHeight + 40],
                        })
                    );

                    poi.marker.bindPopup(data.Content);
                });

                ro.observe($activePoi);

                state.activePopup.addEventListener('popupclose', (e) => {
                    state.activePopup = null;
                    state.activePopupMarker = null;

                    // need to delay to allow the selected POI event
                    // to fire to cater for the scenario where the popup
                    // closed because another POI was selected.
                    window.setTimeout(() => {
                        if (!state.activePopupMarker) {
                            updateQueryString(queryStringParams.poi, '');
                        }
                    }, POPUP_CLOSE_DELAY);
                });
            });
    };

    const mouseOutPoi = (e: L.LeafletEvent) => {
        const selectedPoi = pois.find((poi) => poi.marker === e.target);

        if (!selectedPoi || !selectedPoi.icon?.hoverPath) {
            return;
        }

        const dimensions = getIconDimensions(map, options, selectedPoi.icon);

        selectedPoi.marker.setIcon(
            L.icon({
                iconUrl: selectedPoi.icon.path,

                iconAnchor: [dimensions.width / 2, dimensions.height / 2],
                iconSize: [dimensions.width, dimensions.height],

                popupAnchor: [0, 300],
            })
        );
    };

    const mouseOverPoi = (e: L.LeafletEvent) => {
        const selectedPoi = pois.find((poi) => poi.marker === e.target);

        if (!selectedPoi || !selectedPoi.icon?.hoverPath) {
            return;
        }

        const dimensions = getIconDimensions(map, options, selectedPoi.icon);

        selectedPoi.marker.setIcon(
            L.icon({
                iconUrl: selectedPoi.icon.hoverPath,

                iconAnchor: [dimensions.width / 2, dimensions.height / 2],
                iconSize: [dimensions.width, dimensions.height],

                popupAnchor: [0, 300],
            })
        );
    };

    const onSelectPoi = (e: L.LeafletEvent) => {
        const selectedPoi = pois.find((poi) => poi.marker === e.target);

        if (!selectedPoi) {
            return;
        }

        selectPoi(selectedPoi);
    };

    const removeActivePopup = () => {
        state.activePopupMarker = null;

        if (!state.activePopup) {
            return;
        }

        state.activePopup.remove();
        state.activePopup = null;
    };

    const selectPoi = (
        selectedPoi: IMapMarker,
        shouldUpdateUrl: boolean = true
    ) => {
        state.activePopupMarker = selectedPoi;

        fetchPoiContent(selectedPoi);
        trackPoiSelect(selectedPoi);

        if (shouldUpdateUrl) {
            updateUrl(selectedPoi);
        }
    };

    const trackPoiSelect = (poi: IMapMarker) => {
        if (
            (!window.gtag && !window.ga) ||
            !analytics?.poiSelectEventAction ||
            !analytics?.poiSelectEventCategory
        ) {
            return;
        }

        if (window.gtag) {
            window.gtag('event', analytics.poiSelectEventAction, {
                event_category: analytics.poiSelectEventAction,
                event_label: poi.title,
            });
        }

        if (window.ga) {
            window.ga('send', {
                hitType: 'event',
                eventCategory: analytics.poiSelectEventCategory,
                eventAction: analytics.poiSelectEventAction,
                eventLabel: poi.title,
            });
        }
    };

    const updateUrl = (poi: IMapMarker) => {
        updateQueryString(queryStringParams.poi, poi.contentItemId);
    };

    const initialPoi = getQueryString(queryStringParams.poi);

    for (const poi of pois) {
        poi.marker.addEventListener('click', onSelectPoi);

        if (poi.icon?.hoverPath) {
            poi.marker.addEventListener('mouseover', mouseOverPoi);
            poi.marker.addEventListener('mouseout', mouseOutPoi);
        }

        if (initialPoi && poi.contentItemId === initialPoi) {
            selectPoi(poi);
        }
    }

    map.on('zoomend', () => {
        for (const poi of pois) {
            if (!poi.icon) {
                continue;
            }

            const dimensions = getIconDimensions(map, options, poi.icon);

            poi.marker.setIcon(
                L.icon({
                    iconUrl: poi.icon.path,

                    iconAnchor: [dimensions.width / 2, dimensions.height / 2],
                    iconSize: [dimensions.width, dimensions.height],

                    popupAnchor: [0, 300],
                })
            );
        }
    });

    window.addEventListener('popstate', () => {
        const poiId = getQueryString(queryStringParams.poi);

        if (!poiId) {
            removeActivePopup();
            return;
        }

        for (const poi of pois) {
            if (poi.contentItemId === poiId) {
                selectPoi(poi);
                break;
            }
        }
    });
};

export default displayPois;
