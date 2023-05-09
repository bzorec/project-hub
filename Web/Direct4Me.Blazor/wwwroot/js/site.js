function getLightMode() {
    let mode = localStorage.getItem("lightMode");
    return mode === "true";
}

function setLightMode(value) {
    localStorage.setItem("lightMode", value.toString());
    updateColorScheme();
}

function getDarkMode() {
    let mode = localStorage.getItem("darkMode");
    return mode === "true";
}

function setDarkMode(value) {
    localStorage.setItem("darkMode", value.toString());
    updateColorScheme();
}

function updateColorScheme() {
    let isLightMode = getLightMode();
    let isDarkMode = getDarkMode();

    if (isLightMode) {
        document.documentElement.classList.remove("dark-mode");
        document.documentElement.classList.add("light-mode");
    } else if (isDarkMode) {
        document.documentElement.classList.remove("light-mode");
        document.documentElement.classList.add("dark-mode");
    } else {
        document.documentElement.classList.remove("light-mode");
        document.documentElement.classList.remove("dark-mode");
    }
}

updateColorScheme();
