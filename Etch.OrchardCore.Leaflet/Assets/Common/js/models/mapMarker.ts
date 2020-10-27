import * as L from 'leaflet';

export default interface IMapMarker {
    contentItemId: string,
    $editor?: HTMLElement | undefined,
    marker: L.Marker
}
