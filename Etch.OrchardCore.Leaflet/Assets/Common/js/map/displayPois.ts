import * as L from 'leaflet';
import IInitialiseOptions from '../models/initializeOptions';
import addPois from './addPois';

const POPUP_MAX_WIDTH = 640;

const displayPois = (map: L.Map, options: IInitialiseOptions): void => {
    if (!options.pois || options.isManagePOIs) {
        return;
    }

    const pois = addPois(map, JSON.parse(options.pois));

    const fetchPoiContent = (e: L.LeafletEvent) => {
        const selectedPoi = pois.find(poi => poi.marker === e.target);

        if (!selectedPoi) {
            return;
        }

        window.fetch(`${options.poiDisplayUrl}?contentItemId=${options.contentItemId}&poiContentItemId=${selectedPoi.contentItemId}`)
            .then(response => response.json())
            .then(data => {
                selectedPoi.marker.bindPopup(data.Content, {
                    maxWidth: POPUP_MAX_WIDTH
                }).openPopup();
            });
    };

    for (const poi of pois) {
        poi.marker.addEventListener('click', fetchPoiContent);
    }
};

export default displayPois;