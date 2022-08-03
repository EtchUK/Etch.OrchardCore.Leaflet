import * as L from 'leaflet';

import IInitialiseOptions from '../models/initializeOptions';
import IMapMarker from '../models/mapMarker';
import { getQueryString, updateQueryString } from '../utils/url';

import addPois from './addPois';
import { displayPoi, IPoiPopup } from './displayPoi';
import getIconDimensions from './getIconDimensions';

interface IDisplayPopupsState {
    activePopup: IPoiPopup | null;
}

const queryStringParams = {
    poi: 'poi',
};

const displayPois = (map: L.Map, options: IInitialiseOptions): void => {
    if (!options.pois) {
        return;
    }

    const state: IDisplayPopupsState = { activePopup: null };
    const pois = addPois(map, options, JSON.parse(options.pois));

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
        state.activePopup?.remove();
        state.activePopup = null;
    };

    const selectPoi = (
        selectedPoi: IMapMarker,
        shouldUpdateUrl: boolean = true
    ) => {
        state.activePopup = displayPoi(selectedPoi, options);

        if (shouldUpdateUrl) {
            updateUrl(selectedPoi);
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

            if (
                !poi.alwaysDisplay &&
                poi.zoomLevels.indexOf(map.getZoom()) === -1
            ) {
                poi.marker.removeFrom(map);
            } else {
                poi.marker.addTo(map);
            }

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

        if (poiId && state.activePopup?.poi.contentItemId !== poiId) {
            removeActivePopup();
        }

        if (!poiId && state.activePopup) {
            removeActivePopup();
            return;
        }

        for (const poi of pois) {
            if (poi.contentItemId === poiId) {
                selectPoi(poi, false);
                break;
            }
        }
    });
};

export default displayPois;
