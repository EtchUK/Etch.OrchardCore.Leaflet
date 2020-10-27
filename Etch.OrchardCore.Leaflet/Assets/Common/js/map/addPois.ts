import * as L from 'leaflet';
import IMapMarker from '../models/mapMarker';

import IPoi from "../models/poi";
import addPoi from "./addPoi";

const addPois = (map: L.Map, pois: IPoi[]): IMapMarker[] => {
    return pois.map(poi => {
        return addPoi(map, poi);
    });
};

export default addPois;