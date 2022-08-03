import * as L from 'leaflet';
import IIcon from './icon';

export default interface IMapMarker {
    alwaysDisplay: boolean;
    contentItemId: string;
    $editor?: HTMLElement;
    icon?: IIcon;
    marker: L.Marker;
    title?: string;
    zoomLevels: number[];
}
