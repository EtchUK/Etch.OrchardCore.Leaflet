import IIcon from "./icon";

export default interface IPoi {
    contentItemId: string,
    icon: IIcon
    lat: number,
    lng: number,
    title: string,
}
