const TAB_CHANGE_DELAY = 200;

const invalidateOnTabClick = (map: L.Map): void => {
    const $tabs = document.querySelectorAll('.nav-tabs a');

    $tabs.forEach(($tab) => {
        $tab.addEventListener('click', () => {
            window.setTimeout(() => {
                map.invalidateSize();
            }, TAB_CHANGE_DELAY);
        });
    });
};

export default invalidateOnTabClick;
