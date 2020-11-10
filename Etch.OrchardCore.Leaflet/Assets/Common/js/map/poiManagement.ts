import { Modal } from 'bootstrap';
import * as L from 'leaflet';
import { v4 as uuidv4 } from 'uuid';

import addPoi from '../../../Common/js/map/addPoi';
import addPois from '../../../Common/js/map/addPois';
import IIcon from '../../../Common/js/models/icon';
import IInitialiseOptions from '../../../Common/js/models/initializeOptions';
import IMapMarker from '../../../Common/js/models/mapMarker';

const CLICK_DELAY = 10;

const poiManagement = (map: L.Map, options: IInitialiseOptions): void => {
    const $modal = document.querySelector('#modalPoiMarkers');

    const collapseAllEditors = () => {
        const $editors = document.querySelectorAll('.widget-editor');

        for (let i = 0; i < $editors.length; i++) {
            ($editors[i] as HTMLButtonElement).classList.add('collapsed');
        }
    };

    const handleDeletePoi = (e: MouseEvent) => {
        const contentItemId = (e.currentTarget as HTMLButtonElement).getAttribute(
            'data-content-item-id'
        );

        if (!contentItemId) {
            return;
        }

        // slight delay needed to allow for dialog to be rendered
        window.setTimeout(() => {
            document
                .querySelector('#confirmRemoveModal .btn-danger')
                ?.addEventListener('click', () => {
                    const poiToRemove = activePois.find(
                        (poi) => poi.contentItemId === contentItemId
                    );

                    if (poiToRemove) {
                        map.removeLayer(poiToRemove.marker);
                    }
                });
        }, CLICK_DELAY);
    };

    const handleMovePoi = (e: L.LeafletEvent) => {
        const movedPoi = activePois.find((item) => item.marker === e.target);

        if (!movedPoi || !movedPoi.$editor) {
            return;
        }

        (document.getElementsByName(
            `${movedPoi.$editor.id}.PoiPart.Latitude`
        )[0] as HTMLInputElement).value = e.target._latlng.lat.toString();
        (document.getElementsByName(
            `${movedPoi.$editor.id}.PoiPart.Longitude`
        )[0] as HTMLInputElement).value = e.target._latlng.lng.toString();
    };

    const handleSelectPoi = (e: L.LeafletEvent) => {
        const movedPoi = activePois.find((item) => item.marker === e.target);

        if (!movedPoi || !movedPoi.$editor) {
            return;
        }

        showPoiEditor(movedPoi.$editor);
    };

    const showPoiEditor = ($editor: HTMLElement): void => {
        collapseAllEditors();

        $editor.querySelector('.widget-editor')?.classList.remove('collapsed');
        $editor.scrollIntoView();
    };

    map.on('click', (e: L.LeafletMouseEvent) => {
        const $addPoiButtons = document.querySelectorAll(
            '#modalPoiMarkers .add-poi'
        );

        const handleAddNewPoi = (ev: MouseEvent) => {
            const $btn = ev.target as HTMLButtonElement;
            const targetId = $btn.getAttribute('data-target-id') || '';
            const $placeholder = document.querySelector(`#${targetId}`);

            const settings = {
                contentTypesName: $btn.getAttribute('data-contenttypes-name'),
                createEditorUrl: $placeholder?.getAttribute(
                    'data-buildeditorurl'
                ),
                partName: $btn.getAttribute('data-part-name'),
                parentContentType: $btn.getAttribute(
                    'data-parent-content-type'
                ),
                prefix: uuidv4(),
                prefixesName: $btn.getAttribute('data-prefixes-name'),
                targetId,
                type: $btn.getAttribute('data-widget-type'),
            };

            window
                .fetch(
                    settings.createEditorUrl +
                        '?id=' +
                        settings.type +
                        '&prefix=' +
                        settings.prefix +
                        '&prefixesName=' +
                        settings.prefixesName +
                        '&contentTypesName=' +
                        settings.contentTypesName +
                        '&targetId=' +
                        settings.targetId +
                        '&parentContentType=' +
                        settings.parentContentType +
                        '&partName=' +
                        settings.partName
                )
                .then((response) => response.json())
                .then((result) => {
                    // add POI content item editor to collection
                    if ($placeholder) {
                        const $div = document.createElement('div');
                        $div.innerHTML = result.Content;
                        $placeholder.appendChild(
                            $div.firstElementChild as Node
                        );
                    }

                    // any scripts required for editor should be loaded
                    $(result.Scripts)
                        .filter('script')
                        .each(function () {
                            $.globalEval(
                                this.text ||
                                    this.textContent ||
                                    this.innerHTML ||
                                    ''
                            );
                        });

                    const $newPoi: HTMLElement = $placeholder?.children[
                        $placeholder.children.length - 1
                    ] as HTMLElement;

                    const poi = addPoi(map, {
                        contentItemId:
                            $newPoi.getAttribute('data-content-item-id') || '',
                        icon: generateIcon(ev.target as HTMLButtonElement),
                        lat: e.latlng.lat,
                        lng: e.latlng.lng,
                    });

                    poi.$editor = $newPoi;

                    // add marker to map
                    activePois.push(poi);

                    // set lat/lng values on input for PoiPart
                    (document.getElementsByName(
                        `${$newPoi.id}.PoiPart.Latitude`
                    )[0] as HTMLInputElement).value = e.latlng.lat.toString();
                    (document.getElementsByName(
                        `${$newPoi.id}.PoiPart.Longitude`
                    )[0] as HTMLInputElement).value = e.latlng.lng.toString();

                    // show editor for poi
                    showPoiEditor(poi.$editor);

                    // add event listener for deleting poi from bag
                    ($newPoi.querySelector(
                        '.poi-delete'
                    ) as HTMLButtonElement)?.addEventListener(
                        'click',
                        handleDeletePoi
                    );

                    // add event listener for interaction with poi
                    poi.marker.addEventListener('drag', handleMovePoi);
                    poi.marker.addEventListener('click', handleSelectPoi);
                });

            if ($modal) {
                new Modal($modal).hide();
            }

            dispose();
        };

        const dispose = () => {
            for (let i = 0; i < $addPoiButtons.length; i++) {
                ($addPoiButtons[i] as HTMLButtonElement).removeEventListener(
                    'click',
                    handleAddNewPoi
                );
            }
        };

        const generateIcon = ($btn: HTMLButtonElement): IIcon => {
            return {
                height: parseInt(
                    $btn.getAttribute('data-icon-marker-height')?.toString() ??
                        '0',
                    10
                ),
                path: $btn.getAttribute('data-icon-marker-path') || '',
                width: parseInt(
                    $btn.getAttribute('data-icon-marker-width')?.toString() ??
                        '0',
                    10
                ),
                zoomRatio: 1,
            };
        };

        dispose();

        for (let i = 0; i < $addPoiButtons.length; i++) {
            ($addPoiButtons[i] as HTMLButtonElement).addEventListener(
                'click',
                handleAddNewPoi
            );
        }

        if ($modal) {
            new Modal($modal).show();
        }
    });

    let activePois: IMapMarker[];

    if (options.pois) {
        activePois = addPois(map, JSON.parse(options.pois));

        for (const item of activePois) {
            item.$editor = document.querySelector(
                `div[data-content-item-id="${item.contentItemId}"]`
            ) as HTMLElement;
            item.marker.addEventListener('drag', handleMovePoi);
            item.marker.addEventListener('click', handleSelectPoi);
        }
    }

    const $deletePoiButtons = document.querySelectorAll('.poi-delete');

    for (let i = 0; i < $deletePoiButtons.length; i++) {
        ($deletePoiButtons[i] as HTMLButtonElement).addEventListener(
            'click',
            handleDeletePoi
        );
    }
};

export default poiManagement;
