
var app = function () {

    var subMenuItemOpene = false;

    /**
     * Initialize google maps and add it to document
     * @param {any} apiKey
     */
    /*async function initMap(apiKey) {

        // re-examian as we have direct acces to scripts sections.
        // Should be able to load google maps api from scripts section


        //  Setup html doc
        const parentElement = document.getElementById(`mapHolder`); // a <div>
        const script = document.createElement(`script`);
        script.src = `https://maps.googleapis.com/maps/api/js?key=${apiKey}`;
        script.async = true;
        script.defer = true;
        parentElement.insertBefore(script, null);
        script.onload = async function () { }

    }*/

    // https://gabrieleromanato.name/javascript-how-to-use-the-google-maps-api-with-promises-and-async-await
    /**
     * Use google maps library to geocode an address returning the lattidude and longitiude of this address.
     * @param {any} address
     * @returns
     */
    function getCoordinates(address) {

        return new Promise((resolve, reject) => {
            const geocoder = new google.maps.Geocoder();
            geocoder.geocode({ address: address }, (results, status) => {
                if (status === 'OK') {
                    console.log("Latitude: " + results[0].geometry.location.lat())
                    console.log("Longitude: " + results[0].geometry.location.lng())

                    //document.getElementById('station-lat').value = results[0].geometry.location.lat();
                    //document.getElementById('station-long').value = results[0].geometry.location.lng();

                    var results = {
                        "latitude": results[0].geometry.location.lat(), "longitude": results[0].geometry.location.lng()
                    };

                    resolve(results);

                } {
                    //reject(status);
                }
            });
        });
    }

    /**
     * Helper function for reteving the geodata of an address from a form
     */
    function getCoordinateValues() {
        var latValue = document.getElementById("station-lat").value;
        var longtValue = document.getElementById("station-long").value
    }

    /**
     * Function for initializing an instance of a google map on the webpages
     * @param {any} lat
     * @param {any} long
     * @param {any} mapId
     * @param {any} title
     */
    async function initMapForPetrolStation(lat, long, mapId, title) {
        // Request needed libraries.
        const { Map } = await google.maps.importLibrary("maps");
        const { AdvancedMarkerElement } = await google.maps.importLibrary("marker");
        const map = new Map(document.getElementById("google-map-content"), {
            center: { lat: lat, lng: long },
            zoom: 16,
            mapId: mapId,
        });
        const marker = new AdvancedMarkerElement({
            map,
            position: { lat: lat, lng: long },
            title: title
        });
    }

    /**
     * Funtion to check if the google map api has been set for the webpage.
     * @returns @boolen
     */
    function mapsScriptSet() {
        var isSet = document.getElementById('mapScripLoaded').value
        return isSet == "true" ? true : false
    }

    /**
     * Funtion to check if the google map api has been set for the webpage.
     * @param {any} val
     */
    function setMapScriptStatus(val) {
        var isSet = document.getElementById('mapScripLoaded').value = val;
    }

    /**
     * Dispaly station details in popup
     * @param {any} stringStation
     * @param {any} overlayElement
     * @param {any} classRemove
     * @param {any} classHide
     */
    function previewStation(stringStation, overlayElement, classRemove, classHide) {

        // Not in json format
        if (stringStation === undefined) {
            // initial script load,ignore
        } else {
            //dialog_title
            document.getElementById("dialog_title").textContent = stringStation.StationName;
            //	dialog_stationName
            document.getElementById("dialog_stationName").value = stringStation.StationName;
            //	dialog_stationAddress
            document.getElementById("dialog_stationAddress").value = stringStation.StationAddress;
            //	dialog_stationId
            document.getElementById("dialog_stationId").value = stringStation.Id;
            //	dialog_stationAddedDate
            document.getElementById("dialog_stationAddedDate").value = new Date(stringStation.Added);
            //	dialog_stationPostCode
            document.getElementById("dialog_stationPostCode").value = stringStation.StationPostcode;
            //	dialog_stationLattitude
            document.getElementById("dialog_stationLattitude").value = stringStation.Latitude;
            //	dialog_stationLongitude
            document.getElementById("dialog_stationLongitude").value = stringStation.Longitude;
            //	dialog_stationOnline
            document.getElementById("dialog_stationOnline").checked = stringStation.StationOnline;


            overlayElement.classList.remove(classRemove);
            overlayElement.classList.add(classHide);
            //console.log(stringStation);
        }        
    }

    /**
     * Target / respond to menu drop down click events.
     * @param {any} dropdownElement
     */
    function _targetDropDown(dropdownElement) {
        dropdownElement.addEventListener('click', (e) => {
            if (dropdownElement.classList.contains('closed')) {
                dropdownElement.classList.remove('closed')
            } else {
                dropdownElement.classList.add('closed')
            }
        })

    }

    function _displaySubMenu(target) {

        if (target.classList.contains('closed')) {
            target.classList.remove('closed')
        } else {
            return;
        }
    }
    

    return {
        /*initMap: initMap,*/
        getCoordinates: getCoordinates,
        getCoordinateValues: getCoordinateValues,
        initMapForPetrolStation: initMapForPetrolStation,
        mapsScriptSet: mapsScriptSet,
        setMapScriptStatus: setMapScriptStatus,
        previewStation: previewStation,
        targetDropDown: _targetDropDown,
        displaySubMenu: _displaySubMenu
    }
}