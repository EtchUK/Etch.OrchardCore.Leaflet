import '../css/index.scss';

import * as L from 'leaflet';
import 'leaflet-deepzoom';
import 'leaflet-css';

import IInitializeOptions from './models/initializeOptions';

declare global {
    interface Window {
        initializeMap: (options: IInitializeOptions) => void;
        map: L.Map | undefined;
    }
}

window.initializeMap = (options: IInitializeOptions) => {
    const map = L.map(options.domId, {
        maxBoundsViscosity: 1.0,
        maxZoom: 14,
        minZoom: 8,
        zoomControl: true,
    }).setView(new L.LatLng(0, 0), 14);

    // Workaround for global map referenced in deepzoom init
    // https://github.com/alfarisi/leaflet-deepzoom/issues/8
    window.map = map;
    const dzLayer = (L.tileLayer as any)
        .deepzoom(options.tileRoot, {
            width: 15000,
            height: 7000,
        })
        .addTo(map);
    window.map = undefined;

    map.fitBounds(dzLayer.options.bounds);
    map.setMaxBounds(dzLayer.options.bounds);
};
