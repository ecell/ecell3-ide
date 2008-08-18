from django.utils.translation import ugettext_lazy as _
from django import forms
from django.conf import settings
from django.forms.util import ValidationError

class SBMLUploadForm(forms.Form):
    file = forms.FileField(label = _(u'SBML file'))

    def __new__(cls, *args):
        retval = super(SBMLUploadForm, cls).__new__(cls, *args)
        setattr(retval, 'default_error_messages', {
            'file_too_big': _(u'File size must not exceed %d bytes')
            })
        return retval

    def __init__(self, *args, **kwargs):
        forms.Form.__init__(self, *args, **kwargs) 
        error_messages = kwargs.get('error_messages', {})
        error_messages.update(self.default_error_messages)
        self.error_messages = error_messages

    def as_div(self, klass = 'form-field'):
        "Returns this form rendered as HTML <div>s."
        return self._html_output(u'<div class="' + klass + '"><div class="label_and_errorlist">%(label)s%(errors)s</div>%(field)s%(help_text)s</div>', u'%s', '</div>', u' %s', False)

    def clean(self):
        cleaned_data = forms.Form.clean(self)
        if cleaned_data.has_key('file') and cleaned_data['file'] != None:
            if cleaned_data['file'].size > settings.FILE_UPLOAD_MAX_SIZE:
                raise ValidationError(
                    self.error_messages['file_too_big'] % (settings.FILE_UPLOAD_MAX_SIZE, )
                    )
        return cleaned_data

