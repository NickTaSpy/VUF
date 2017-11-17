"use strict";
/// <reference path="types-gt-mp/index.d.ts" />
var cefEscapable;
var mainBrowser = null;
var path = "Clientside/TeamsPage/TeamSelection.html";
API.onServerEventTrigger.connect(function (eventName, args) {
    if (eventName == "onSwitch") {
        if (args[0] == false) {
            cefEscapable = false;
            load();
        }
        else {
            toggle();
        }
    }
});
API.onKeyUp.connect(function (sender, e) {
    if (e.KeyCode === Keys.Up) {
        toggle();
    }
});
function toggle() {
    if (mainBrowser == null) {
        load();
    }
    else {
        destroy();
    }
}
function load() {
    API.sendNotification("Opening team selection menu...");
    var res = API.getScreenResolutionMaintainRatio();
    mainBrowser = API.createCefBrowser(res.Width - 205, res.Height - 375, true);
    API.setCefBrowserPosition(mainBrowser, 100, 100);
    API.waitUntilCefBrowserInit(mainBrowser);
    API.showCursor(true);
    API.loadPageCefBrowser(mainBrowser, path);
}
function destroy() {
    if (cefEscapable == true) {
        API.destroyCefBrowser(mainBrowser);
        mainBrowser = null;
        API.showCursor(false);
    }
    else {
        API.sendNotification("You can't escape this menu right now.");
    }
}
function onSwitch(team) {
    if (API.getWorldSyncedData(team + "FULL") == false) {
        API.triggerServerEvent("Switch", team);
        API.playSoundFrontEnd("OK", "HUD_FRONTEND_DEFAULT_SOUNDSET");
        cefEscapable = true;
        destroy();
    }
    else {
        API.playSoundFrontEnd("ERROR", "HUD_FRONTEND_TATTOO_SHOP_SOUNDSET");
    }
}
