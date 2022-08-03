import IIcon from './icon';

export default interface IPoi {
    alwaysDisplay: boolean;
    contentItemId: string;
    icon: IIcon;
    lat: number;
    lng: number;
    title?: string;
    zoomLevels: number[];
}
