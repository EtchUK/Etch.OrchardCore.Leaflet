import { Modal } from 'bootstrap';
import * as L from 'leaflet';

import addPoi from '../../../Common/js/map/addPoi';
import addPois from '../../../Common/js/map/addPois';
import IIcon from '../../../Common/js/models/icon';
import IInitialiseOptions from "../../../Common/js/models/initializeOptions";
import guid from '../../../Common/js/utils/guid';

const poiManagement = (map: L.Map, options: IInitialiseOptions): void => {
    if (!options.isManagePOIs) {
        return;
    }

    if (options.pois) {
        addPois(map, JSON.parse(options.pois));
    }

    const $modal = document.querySelector('#modalPoiMarkers')

    map.on('click', (e: L.LeafletMouseEvent) => {
        const $addPoiButtons = document.querySelectorAll('#modalPoiMarkers .add-poi');

        const handlePOISelect = (ev: MouseEvent) => {
            const $btn = (ev.target as HTMLButtonElement)
            const targetId = $btn.getAttribute('data-target-id') || '';
            const $placeholder = document.querySelector(`#${targetId}`);

            addPoi(map, {
                icon: generateIcon(ev.target as HTMLButtonElement),
                lat: e.latlng.lat,
                lng: e.latlng.lng
            });

            const settings = {
                contentTypesName: $btn.getAttribute('data-contenttypes-name'),
                createEditorUrl: $placeholder?.getAttribute('data-buildeditorurl'),
                partName: $btn.getAttribute('data-part-name'),
                parentContentType: $btn.getAttribute('data-parent-content-type'),
                prefix: guid(),
                prefixesName: $btn.getAttribute('data-prefixes-name'),
                targetId,
                type: $btn.getAttribute('data-widget-type')
            }

            window.fetch(settings.createEditorUrl + "?id=" + settings.type + "&prefix=" + settings.prefix + "&prefixesName=" + settings.prefixesName + "&contentTypesName=" + settings.contentTypesName + "&targetId=" + settings.targetId  + "&parentContentType=" + settings.parentContentType +"&partName=" + settings.partName)
                .then(response => response.json())
                .then((result) => {
                    if ($placeholder) {
                        const $div = document.createElement('div');
                        $div.innerHTML = result.Content;
                        $placeholder.appendChild($div.firstElementChild as Node);
                    }

                    $(result.Scripts).filter('script').each(function () {
                        $.globalEval(this.text || this.textContent || this.innerHTML || '');
                    });

                    const $newPoi: HTMLElement = $placeholder?.children[$placeholder.children.length - 1] as HTMLElement;

                    (document.getElementsByName(`${$newPoi.id}.PoiPart.Latitude`)[0] as HTMLInputElement).value = e.latlng.lat.toString();
                    (document.getElementsByName(`${$newPoi.id}.PoiPart.Longitude`)[0] as HTMLInputElement).value = e.latlng.lng.toString();

                    ($placeholder?.parentElement?.querySelector('.poi-empty') as Element).classList.add('d-none');
                });

            if ($modal) {
                new Modal($modal).hide();
            }

            dispose();
        };

        const dispose = () => {
            for (let i = 0; i < $addPoiButtons.length; i++) {
                ($addPoiButtons[i] as HTMLButtonElement).removeEventListener('click', handlePOISelect);
            }
        }
        
        const generateIcon = ($btn: HTMLButtonElement): IIcon => {
            return  {
                height: parseInt($btn.getAttribute('data-icon-marker-height')?.toString() ?? '0', 10),
                path: $btn.getAttribute('data-icon-marker-path') || '',
                width: parseInt($btn.getAttribute('data-icon-marker-width')?.toString() ?? '0', 10),
            };
        };

        dispose();

        for (let i = 0; i < $addPoiButtons.length; i++) {
            ($addPoiButtons[i] as HTMLButtonElement).addEventListener('click', handlePOISelect);
        }

        if ($modal) {
            new Modal($modal).show();
        }
    });
};

export default poiManagement;