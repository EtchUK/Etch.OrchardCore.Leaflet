import * as L from 'leaflet';

import IPoi from "../models/poi";
import addPoi from "./addPoi";

const addPois = (map: L.Map, pois: IPoi[]): L.Marker[] => {
    return pois.map(poi => {
        return addPoi(map, poi);
    });
};

export default addPois;