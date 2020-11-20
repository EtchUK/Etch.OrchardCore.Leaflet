import IPoi from '../models/poi';

import * as L from 'leaflet';
import IMapMarker from '../models/mapMarker';
import IInitialiseOptions from '../models/initializeOptions';
import getIconDimensions from './getIconDimensions';

const addPoi = (
    map: L.Map,
    options: IInitialiseOptions,
    poi: IPoi
): IMapMarker => {
    const dimensions = getIconDimensions(map, options, poi.icon);

    const icon = L.icon({
        iconUrl: poi.icon.path,

        iconAnchor: [dimensions.width / 2, dimensions.height / 2],
        iconSize: [dimensions.width, dimensions.height],
    });

    return {
        contentItemId: poi.contentItemId,
        icon: poi.icon,
        marker: L.marker([poi.lat, poi.lng], {
            draggable: options.draggableMarkers,
            icon,
        }).addTo(map),
        title: poi.title,
    };
};

export default addPoi;
