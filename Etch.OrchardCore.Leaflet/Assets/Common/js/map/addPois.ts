import * as L from 'leaflet';

import IInitialiseOptions from '../models/initializeOptions';
import IMapMarker from '../models/mapMarker';
import IPoi from '../models/poi';

import addPoi from './addPoi';

const addPois = (
    map: L.Map,
    options: IInitialiseOptions,
    pois: IPoi[]
): IMapMarker[] => {
    return pois.map((poi) => {
        return addPoi(map, options, poi);
    });
};

export default addPois;
