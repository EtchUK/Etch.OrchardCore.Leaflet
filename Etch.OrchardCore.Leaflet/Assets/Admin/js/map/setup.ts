import '../../css/index.scss';

import * as L from 'leaflet';

import initialize from '../../../Common/js/initialize';
import IInitializeOptions from '../../../Common/js/models/initializeOptions';

declare global
 {
    interface Window {
        initializeMap: (options: IInitializeOptions) => void;
        map: L.Map | undefined;
    }
}

const setup = (): void => {
    window.initializeMap = initialize
};

export default setup;