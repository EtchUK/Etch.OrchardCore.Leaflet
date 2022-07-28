import * as L from 'leaflet';

import initialize from '../../../Common/js/initialize';
import IInitializeOptions from '../../../Common/js/models/initializeOptions';
import invalidateOnTabClick from '../../../Common/js/map/invalidateOnTabClick';
import poiManagement from '../../../Common/js/map/poiManagement';

declare global {
    interface Window {
        initializeMap: (options: IInitializeOptions) => void;
        map: L.Map | undefined;
    }
}

const setup = (): void => {
    window.initializeMap = (options: IInitializeOptions) => {
        const map = initialize(options);
        invalidateOnTabClick(map);
        poiManagement(map, options);
    };
};

export default setup;
