import * as L from 'leaflet';
import 'leaflet-deepzoom';
import 'leaflet-css';

import IInitializeOptions from './models/initializeOptions';

const initialize = (options: IInitializeOptions): L.Map => {
    const map = L.map(options.domId, {
        attributionControl: false,
        maxZoom: options.maxZoom || 14,
        minZoom: options.minZoom || 8,
        zoomControl: true,
    }).setView(new L.LatLng(0, 0), options.initialZoom || 11);

    // Workaround for global map referenced in deepzoom init
    // https://github.com/alfarisi/leaflet-deepzoom/issues/8
    window.map = map;
    const dzLayer: L.TileLayer.DeepZoom = L.tileLayer
        .deepzoom(options.tileRoot, {
            width: options.width,
            height: options.height,
        })
        .addTo(map);
    window.map = undefined;

    if (dzLayer.options.bounds instanceof L.LatLngBounds) {
        map.fitBounds(dzLayer.options.bounds);
        map.setMaxBounds(dzLayer.options.bounds);
    }
    
    map.setZoom(options.initialZoom || 11);

    return map;
};

export default initialize;