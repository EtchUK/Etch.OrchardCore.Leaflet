import * as L from 'leaflet';

export default interface IMapMarker {
    contentItemId: string,
    $editor?: HTMLElement,
    marker: L.Marker
    title: string
}
