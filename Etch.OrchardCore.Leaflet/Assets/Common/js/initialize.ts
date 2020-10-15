import * as L from 'leaflet';
import 'leaflet-deepzoom';
import 'leaflet-css';

import IInitializeOptions from '../../Common/js/models/initializeOptions';

const initialize = (options: IInitializeOptions) => {
    const map = L.map(options.domId, {
        attributionControl: false,
        maxBoundsViscosity: 1.0,
        maxZoom: 14,
        minZoom: 10,
        zoomControl: true,
    }).setView(new L.LatLng(0, 0), 12);

    // Workaround for global map referenced in deepzoom init
    // https://github.com/alfarisi/leaflet-deepzoom/issues/8
    window.map = map;
    const dzLayer = (L.tileLayer as any)
        .deepzoom(options.tileRoot, {
            width: options.width,
            height: options.height,
        })
        .addTo(map);
    window.map = undefined;

    map.fitBounds(dzLayer.options.bounds);
    map.setMaxBounds(dzLayer.options.bounds);
    map.setZoom(12);
};

export default initialize;