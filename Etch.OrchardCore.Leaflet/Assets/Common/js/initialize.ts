import * as L from 'leaflet';
import 'leaflet-deepzoom';
import './removeGridLines';

import IInitializeOptions from './models/initializeOptions';
import { getQueryString, updateQueryString } from './utils/url';

const queryStringParams = {
    zoom: 'zoom',
};

const getInitialZoom = (options: IInitializeOptions): number => {
    try {
        const zoomParam = getQueryString(queryStringParams.zoom);

        if (zoomParam !== null) {
            return parseInt(zoomParam, 10);
        }
    } catch {
        // do nothing
    }

    if (options.initialZoom) {
        return options.initialZoom;
    }

    return 11;
};

const initialize = (options: IInitializeOptions): L.Map => {
    const initialZoomLevel = getInitialZoom(options);

    const map = L.map(options.domId, {
        attributionControl: false,
        maxZoom: options.maxZoom || 14,
        minZoom: options.minZoom || 8,
        zoomControl: true,
    }).setView(new L.LatLng(0, 0), initialZoomLevel);

    (map.zoomControl as any).setPosition(options.zoomControlPosition);

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
        // map.setMaxBounds(dzLayer.options.bounds);
    }

    map.setZoom(initialZoomLevel);

    if (options.deepLinking) {
        map.on('zoomend', () => {
            updateQueryString(queryStringParams.zoom, map.getZoom().toString());
        });
    }

    window.addEventListener('popstate', () => {
        const zoomLevel = getQueryString(queryStringParams.zoom);

        if (zoomLevel) {
            map.setZoom(parseInt(zoomLevel, 10));
        }
    });

    return map;
};

export default initialize;
