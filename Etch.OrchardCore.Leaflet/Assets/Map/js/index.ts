﻿import * as L from 'leaflet';

import initialize from '../../Common/js/initialize';
import IInitializeOptions from '../../Common/js/models/initializeOptions';
import displayPois from '../../Common/js/map/displayPois';

declare global {
    interface Window {
        dataLayer: any;
        ga: GoogleAnalytics;
        gtag: Gtag.Gtag;
        initializeMap: (options: IInitializeOptions) => void;
        map: L.Map | undefined;
    }
}

window.initializeMap = (options: IInitializeOptions) => {
    const map = initialize(options);

    displayPois(map, options);
};
