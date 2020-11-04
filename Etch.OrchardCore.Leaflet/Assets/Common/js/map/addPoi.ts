import IPoi from "../models/poi";

import * as L from 'leaflet';
import IMapMarker from "../models/mapMarker";

const addPoi = (map: L.Map, poi: IPoi): IMapMarker => {
    const icon = L.icon({
        iconUrl: poi.icon.path,

        iconAnchor: [poi.icon.width / 2, poi.icon.height / 2],
        iconSize: [poi.icon.width, poi.icon.height],
    });

    return {
        contentItemId: poi.contentItemId,
        icon: poi.icon,
        marker: L.marker([poi.lat, poi.lng], { draggable: true, icon }).addTo(map),
        title: poi.title
    };
};

export default addPoi;