import IAnalytics from '../models/analytics';
import IInitialiseOptions from '../models/initializeOptions';
import IMapMarker from '../models/mapMarker';
import { executeScripts } from '../utils/scripts';
import { updateQueryString } from '../utils/url';

export interface IPoiPopup {
    poi: IMapMarker;
    remove: () => void;
}

export const displayPoi = (
    poi: IMapMarker,
    options: IInitialiseOptions
): IPoiPopup => {
    const analytics: IAnalytics | null = options.analytics
        ? JSON.parse(options.analytics)
        : null;

    const checkClickOutside = (e: MouseEvent) => {
        if ($modal === e.target) {
            removePopup();
        }
    };

    const checkForEscape = (e: KeyboardEvent) => {
        if (e.key === 'Escape') {
            removePopup();
        }
    };

    const createModal = (): HTMLElement => {
        const $el = document.createElement('div');
        $el.classList.add('leaflet-popup-modal');

        $el.innerHTML = `<div class="leaflet-popup-modal-wrapper">
    <button type="button" class="leaflet-popup-close-button"><span>×</span></button>
    <div class="leaflet-popup-modal-wrapper-background"></div>
    <div class="leaflet-popup-modal-content js-leaflet-popup-modal-content"></div>
</div>`;

        return $el;
    };

    const fetchContent = () => {
        const $content = $modal.querySelector(
            '.js-leaflet-popup-modal-content'
        );

        window
            .fetch(
                `${options.poiDisplayUrl}?contentItemId=${options.contentItemId}&poiContentItemId=${poi.contentItemId}`
            )
            .then((response) => response.json())
            .then((data) => {
                if ($content !== null) {
                    $content.innerHTML = $content.innerHTML.concat(
                        data.Content
                    );

                    executeScripts($content);
                }

                document
                    .querySelector('html')
                    ?.classList.add('is-displaying-poi');

                document.querySelector('body')?.appendChild($modal);

                $modal
                    .querySelector('.leaflet-popup-close-button')
                    ?.addEventListener('click', removePopup);

                document.addEventListener('click', checkClickOutside);

                document.addEventListener('keyup', checkForEscape);
            });
    };

    const removePopup = (shouldUpdateUrl: boolean = true) => {
        $modal.remove();

        if (shouldUpdateUrl) {
            updateQueryString('poi', '');
        }

        document.querySelector('html')?.classList.remove('is-displaying-poi');

        document.removeEventListener('click', checkClickOutside);
        document.removeEventListener('keyup', checkForEscape);
    };

    const track = () => {
        if (
            (!window.gtag && !window.ga) ||
            !analytics?.poiSelectEventAction ||
            !analytics?.poiSelectEventCategory
        ) {
            return;
        }

        if (window.gtag) {
            window.gtag('event', analytics.poiSelectEventAction, {
                event_category: analytics.poiSelectEventAction,
                event_label: poi.title,
            });
        }

        if (window.ga) {
            window.ga('send', {
                hitType: 'event',
                eventCategory: analytics.poiSelectEventCategory,
                eventAction: analytics.poiSelectEventAction,
                eventLabel: poi.title,
            });
        }
    };

    const $modal = createModal();

    fetchContent();
    track();

    return {
        poi,
        remove: removePopup,
    };
};
