/**
 * @license Copyright (c) 2003-2022, CKSource Holding sp. z o.o. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function (config) {
    config.toolbarGroups = [
        { name: 'document', groups: ['mode', 'document', 'doctools'] },
        { name: 'clipboard', groups: ['clipboard', 'undo'] },
        { name: 'editing', groups: ['find', 'selection', 'spellchecker', 'editing'] },
        { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
        '/',
        { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi', 'paragraph'] },
        { name: 'links', groups: ['links'] },
        { name: 'insert', groups: ['insert'] }, // Include the 'image2' group for inserting images.
        '/',
        { name: 'styles', groups: ['styles'] },
        { name: 'colors', groups: ['colors'] },
        { name: 'tools', groups: ['tools'] },
        { name: 'others', groups: ['others'] },
        { name: 'about', groups: ['about'] },
        { name: 'imageuploader', groups: ['imageuploader'] },
        { name: 'mathEx', groups: ['MathEx'] },
        { name: 'mathjax', groups: ['mathjax'] },
    ];


    config.removeButtons = 'Print,Preview,ExportPdf,NewPage,Save,Templates,Form,Checkbox,Radio,TextField,Textarea,Select,Button,HiddenField,ImageButton,Language,About,Iframe';


    config.allowedContent = true;
    config.enterMode = CKEDITOR.ENTER_BR;
    config.shiftEnterMode = CKEDITOR.ENTER_P;
    //config.extraPlugins = 'confighelper,pastecode,html5audio,base64image,ckeditor_wiris,pastebase64';
    config.pasteFromWordRemoveFontStyles = true;
    config.pasteFromWordRemoveStyles = true;
    config.removePlugins = 'easyimage, cloudservices';
    config.mathJaxLib = '//cdnjs.cloudflare.com/ajax/libs/mathjax/2.7.4/MathJax.js?config=TeX-AMS_HTML';
    config.mathJaxClass = 'equation';

};
