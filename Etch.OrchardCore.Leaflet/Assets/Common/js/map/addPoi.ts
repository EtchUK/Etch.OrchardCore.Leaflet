import IPoi from "../models/poi";

import * as L from 'leaflet';
import IMapMarker from "../models/mapMarker";

const addPoi = (map: L.Map, poi: IPoi): IMapMarker => {
    const icon = L.icon({
        iconUrl: poi.icon.path,

        iconAnchor: [poi.icon.width / 2, poi.icon.height],
        iconSize: [poi.icon.width, poi.icon.height],
    });

    return {
        contentItemId: poi.contentItemId,
        marker: L.marker([poi.lat, poi.lng], { icon }).addTo(map)
    }
};

export default addPoi;