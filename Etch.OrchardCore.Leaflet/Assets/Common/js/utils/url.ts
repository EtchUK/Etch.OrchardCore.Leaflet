export const getQueryString = (name: string): string | null => {
    return new URL(document.location.href).searchParams.get(name);
};

export const updateQueryString = (name: string, value: string) => {
    const pageUrl = new URL(document.location.href);
    const searchParams = pageUrl.searchParams;

    if (value) {
        searchParams.set(name, value);
    } else {
        searchParams.delete(name);
    }

    const state: any = {};

    searchParams.forEach((paramValue, key) => {
        state[key] = paramValue;
    });

    pageUrl.search = searchParams.toString();
    window.history.pushState(state, document.title, pageUrl.toString());
};
