/**
 * Fixes issue where grid lines will display.
 * https://github.com/Leaflet/Leaflet/issues/3575#issuecomment-150544739
 */

(function () {
    var originalInitTile = L.GridLayer.prototype._initTile;
    L.GridLayer.include({
        _initTile: function (tile) {
            originalInitTile.call(this, tile);

            var tileSize = this.getTileSize();

            tile.style.width = tileSize.x + 1.5 + 'px';
            tile.style.height = tileSize.y + 1.5 + 'px';
        },
    });
})();
