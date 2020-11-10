import * as L from 'leaflet';
import IAnalytics from '../models/analytics';
import IIcon from '../models/icon';
import IInitialiseOptions from '../models/initializeOptions';
import IMapMarker from '../models/mapMarker';
import addPois from './addPois';

const POPUP_MAX_WIDTH = 640;

interface IIconDimensions {
    height: number;
    width: number;
}

const displayPois = (map: L.Map, options: IInitialiseOptions): void => {
    if (!options.pois) {
        return;
    }

    const analytics: IAnalytics | null = options.analytics
        ? JSON.parse(options.analytics)
        : null;
    const pois = addPois(map, JSON.parse(options.pois));

    const fetchPoiContent = (poi: IMapMarker) => {
        window
            .fetch(
                `${options.poiDisplayUrl}?contentItemId=${options.contentItemId}&poiContentItemId=${poi.contentItemId}`
            )
            .then((response) => response.json())
            .then((data) => {
                poi.marker
                    .bindPopup(data.Content, {
                        maxWidth: POPUP_MAX_WIDTH,
                    })
                    .openPopup();
            });
    };

    const getIconDimensions = (icon: IIcon): IIconDimensions => {
        const heightStep = icon.height * icon.zoomRatio - icon.height;
        const widthStep = icon.width * icon.zoomRatio - icon.width;
        const levelDifference = Math.abs(map.getZoom() - options.minZoom);

        return {
            height: icon.height + heightStep * levelDifference,
            width: icon.width + widthStep * levelDifference,
        };
    };

    const mouseOutPoi = (e: L.LeafletEvent) => {
        const selectedPoi = pois.find((poi) => poi.marker === e.target);

        if (!selectedPoi || !selectedPoi.icon?.hoverPath) {
            return;
        }

        const dimensions = getIconDimensions(selectedPoi.icon);

        selectedPoi.marker.setIcon(
            L.icon({
                iconUrl: selectedPoi.icon.path,

                iconAnchor: [dimensions.width / 2, dimensions.height / 2],
                iconSize: [dimensions.width, dimensions.height],
            })
        );
    };

    const mouseOverPoi = (e: L.LeafletEvent) => {
        const selectedPoi = pois.find((poi) => poi.marker === e.target);

        if (!selectedPoi || !selectedPoi.icon?.hoverPath) {
            return;
        }

        const dimensions = getIconDimensions(selectedPoi.icon);

        selectedPoi.marker.setIcon(
            L.icon({
                iconUrl: selectedPoi.icon.hoverPath,

                iconAnchor: [dimensions.width / 2, dimensions.height / 2],
                iconSize: [dimensions.width, dimensions.height],
            })
        );
    };

    const selectPoi = (e: L.LeafletEvent) => {
        const selectedPoi = pois.find((poi) => poi.marker === e.target);

        if (!selectedPoi) {
            return;
        }

        fetchPoiContent(selectedPoi);
        trackPoiSelect(selectedPoi);
    };

    const trackPoiSelect = (poi: IMapMarker) => {
        if (
            !window.ga ||
            !analytics?.poiSelectEventAction ||
            !analytics?.poiSelectEventCategory
        ) {
            return;
        }

        window.ga('send', {
            hitType: 'event',
            eventCategory: analytics.poiSelectEventCategory,
            eventAction: analytics.poiSelectEventAction,
            eventLabel: poi.title,
        });
    };

    for (const poi of pois) {
        poi.marker.addEventListener('click', selectPoi);

        if (poi.icon?.hoverPath) {
            poi.marker.addEventListener('mouseover', mouseOverPoi);
            poi.marker.addEventListener('mouseout', mouseOutPoi);
        }
    }

    map.on('zoomend', function () {
        for (const poi of pois) {
            if (!poi.icon) {
                continue;
            }

            const dimensions = getIconDimensions(poi.icon);

            poi.marker.setIcon(
                L.icon({
                    iconUrl: poi.icon.path,

                    iconAnchor: [dimensions.width / 2, dimensions.height / 2],
                    iconSize: [dimensions.width, dimensions.height],
                })
            );
        }
    });
};

export default displayPois;
