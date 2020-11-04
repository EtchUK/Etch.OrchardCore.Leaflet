import * as L from 'leaflet';
import IAnalytics from '../models/analytics';
import IInitialiseOptions from '../models/initializeOptions';
import IMapMarker from '../models/mapMarker';
import addPois from './addPois';

const POPUP_MAX_WIDTH = 640;

const displayPois = (map: L.Map, options: IInitialiseOptions): void => {
    if (!options.pois) {
        return;
    }

    const analytics: IAnalytics | null = options.analytics ? JSON.parse(options.analytics) : null;
    const pois = addPois(map, JSON.parse(options.pois));

    const fetchPoiContent = (poi: IMapMarker) => {
        window.fetch(`${options.poiDisplayUrl}?contentItemId=${options.contentItemId}&poiContentItemId=${poi.contentItemId}`)
            .then(response => response.json())
            .then(data => {
                poi.marker.bindPopup(data.Content, {
                    maxWidth: POPUP_MAX_WIDTH
                }).openPopup();
            });
    };

    const selectPoi = (e: L.LeafletEvent) => {
        const selectedPoi = pois.find(poi => poi.marker === e.target);

        if (!selectedPoi) {
            return;
        }

        fetchPoiContent(selectedPoi);
        trackPoiSelect(selectedPoi);
    };

    const trackPoiSelect = (poi: IMapMarker) => {
        if (!window.ga || !analytics?.poiSelectEventAction || !analytics?.poiSelectEventCategory) {
            return;
        }

        window.ga('send', {
            hitType: 'event',
            eventCategory: analytics.poiSelectEventCategory,
            eventAction: analytics.poiSelectEventAction,
            eventLabel: poi.title
        });
    };

    for (const poi of pois) {
        poi.marker.addEventListener('click', selectPoi);
    }
};

export default displayPois;