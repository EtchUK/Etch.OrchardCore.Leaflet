import IInitialiseOptions from "../../../Common/js/models/initializeOptions";

const TAB_CHANGE_DELAY = 200;

const invalidateOnTabClick = (map: L.Map, options: IInitialiseOptions) => {
    if (!options.isAdmin) {
        return;
    }

    const $tabs = document.querySelectorAll('.nav-tabs a');

    for (let i = 0; i < $tabs.length; i++) {
        $tabs[i].addEventListener('click', () => {
            window.setTimeout(() => {
                map.invalidateSize();
            }, TAB_CHANGE_DELAY);
        });
    };
};

export default invalidateOnTabClick;