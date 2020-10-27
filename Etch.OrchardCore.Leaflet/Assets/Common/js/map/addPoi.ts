import IPoi from "../models/poi";

import * as L from 'leaflet';

const addPoi = (map: L.Map, poi: IPoi): L.Marker => {
    const icon = L.icon({
        iconUrl: poi.icon.path,

        iconAnchor: [poi.icon.width / 2, poi.icon.height],
        iconSize: [poi.icon.width, poi.icon.height],
    });

    return L.marker([poi.lat, poi.lng], { icon }).addTo(map);
};

export default addPoi;