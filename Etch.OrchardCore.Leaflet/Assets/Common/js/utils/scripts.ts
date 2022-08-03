export const executeScripts = ($el: Element) => {
    Array.from($el.querySelectorAll('script')).forEach((oldScript) => {
        const newScript = document.createElement('script');

        Array.from(oldScript.attributes).forEach((attr) =>
            newScript.setAttribute(attr.name, attr.value)
        );

        newScript.appendChild(document.createTextNode(oldScript.innerHTML));

        oldScript.parentNode?.replaceChild(newScript, oldScript);
    });
};
