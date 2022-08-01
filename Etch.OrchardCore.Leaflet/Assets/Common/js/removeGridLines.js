/**
 * Fixes issue where grid lines will display.
 * https://github.com/Leaflet/Leaflet/issues/3575#issuecomment-150544739
 */

import * as L from 'leaflet';

(function () {
    const originalInitTile = L.GridLayer.prototype._initTile;
    L.GridLayer.include({
        _initTile: function (tile) {
            originalInitTile.call(this, tile);

            const tileSize = this.getTileSize();

            tile.style.width = tileSize.x + 1.5 + 'px';
            tile.style.height = tileSize.y + 1.5 + 'px';
        },
    });
})();
