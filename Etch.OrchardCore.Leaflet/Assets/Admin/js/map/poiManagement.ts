import * as L from 'leaflet';

import IInitialiseOptions from "../../../Common/js/models/initializeOptions";

const poiManagement = (map: L.Map, options: IInitialiseOptions) => {
    if (!options.isManagePOIs) {
        return;
    }

    map.on('click', (e: L.LeafletMouseEvent) => {
        const $addPoiButtons = document.querySelectorAll('#modalPoiMarkers .add-poi');

        const addPOI = (ev: MouseEvent) => {
            const icon = {
                height: parseInt((ev.target as HTMLButtonElement).getAttribute('data-icon-marker-height')?.toString() ?? '0', 10),
                path: (ev.target as HTMLButtonElement).getAttribute('data-icon-marker-path') || '',
                width: parseInt((ev.target as HTMLButtonElement).getAttribute('data-icon-marker-width')?.toString() ?? '0', 10),
            };

            const markerIcon = L.icon({
                iconUrl: icon.path,

                iconAnchor: [icon.width / 2, icon.height],
                iconSize: [icon.width, icon.height]
            });

            L.marker([e.latlng.lat, e.latlng.lng], { icon: markerIcon }).addTo(map);

            ($('#modalPoiMarkers') as any).modal('hide');

            dispose();
        };

        const dispose = () => {
            for (let i = 0; i < $addPoiButtons.length; i++) {
                ($addPoiButtons[i] as HTMLButtonElement).removeEventListener('click', addPOI);
            }
        }

        dispose();

        for (let i = 0; i < $addPoiButtons.length; i++) {
            ($addPoiButtons[i] as HTMLButtonElement).addEventListener('click', addPOI);
        }

        ($('#modalPoiMarkers') as any).modal();
    });
};

export default poiManagement;