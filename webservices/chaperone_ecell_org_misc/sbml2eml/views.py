import os.path
from django.template import RequestContext
from django.utils.translation import ugettext_lazy as _
from django.http import HttpResponse, HttpResponseRedirect, HttpResponseServerError
from django.shortcuts import *
from django.forms.forms import NON_FIELD_ERRORS
from django.forms.util import ErrorList
from chaperone_ecell_org_misc.sbml2eml.forms import *
from ecell.convertSBML2EML import convertSBML2EML
from django.conf import settings

def show_form(request):
    vars = dict(form = SBMLUploadForm(), title = _(u'SBML to EML converter'))
    return render_to_response('sbml2eml/form.html', vars,
        context_instance = RequestContext(request))

def handle_form(request):
    if request.method == 'POST':
        form = SBMLUploadForm(request.POST, request.FILES)
        vars = dict(form = form)
        if form.is_valid():
            try:
                resp = HttpResponse(
                    content = convertSBML2EML(request.FILES['file'].read()).asString(),
                    mimetype = 'application/x-ecell-model'
                    )
                eml_file_name = os.path.splitext(
                    os.path.basename(request.FILES['file'].filename))[0] + '.eml'
                resp['Content-Disposition'] = 'attachment; filename=%s' % eml_file_name
                return resp
            except Exception, e:
                form._errors[NON_FIELD_ERRORS] = ErrorList([_(u'Conversion failed (attempt to convert a non-SBML file?)')])
        return render_to_response('sbml2eml/form.html', vars,
            context_instance = RequestContext(request))
    elif request.method == "PUT":
        try:
            return HttpResponse(
                content = convertSBML2EML(request.raw_post_data).asString(),
                mimetype = 'application/x-ecell-model',
                )
        except Exception, e:
            return HttpResponseServerError(
                content_type = 'text/plain; charset=%s' %
                    settings.DEFAULT_CHARSET,
                content = unicode(_(u'Conversion failed (attempt to convert a non-SBML file?)'))
                )
    else:
        return show_form(request)
