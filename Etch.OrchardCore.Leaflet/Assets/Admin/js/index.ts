import setupMap from './map/setup';
import storeImageDimensions from './tiles/storeImageDimensions';

setupMap();

document
    .querySelector('.mediaFieldSelectButton')
    ?.addEventListener('click', storeImageDimensions);
