import IIcon from '../models/icon';
import IIconDimensions from '../models/iconDimensions';
import IInitialiseOptions from '../models/initializeOptions';

const getIconDimensions = (
    map: L.Map,
    options: IInitialiseOptions,
    icon: IIcon
): IIconDimensions => {
    const heightStep = icon.height * icon.zoomRatio - icon.height;
    const widthStep = icon.width * icon.zoomRatio - icon.width;
    const levelDifference = Math.abs(map.getZoom() - options.minZoom);

    return {
        height: icon.height + heightStep * levelDifference,
        width: icon.width + widthStep * levelDifference,
    };
};

export default getIconDimensions;
