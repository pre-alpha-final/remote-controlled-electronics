masonryDict = {}

function masonryInitialize(id, options) {
	masonryDict[id] = new Masonry('#' + id, options);
}

function masonryUpdate(id) {
	do {
		let masonryElement = document.getElementById(id);
		var elements = masonryElement.getElementsByClassName("addtomasonry");
		if (elements.length != 0) {
			let element = elements[0];
			masonryDict[id].appended([element]);
			element.classList.remove("addtomasonry");
		}
	} while (elements.length != 0);
	masonryDict[id].layout();
}
