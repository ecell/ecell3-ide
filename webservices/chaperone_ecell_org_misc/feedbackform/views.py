import sys
from django.template import RequestContext
from django.utils.translation import ugettext_lazy as _, activate as activate_lang, get_language
from django.http import HttpResponse, HttpResponseRedirect, HttpResponseServerError
from django.shortcuts import *
from django.forms.forms import NON_FIELD_ERRORS
from django.forms.util import ErrorList
from chaperone_ecell_org_misc.feedbackform.forms import *
from ecell.convertSBML2EML import convertSBML2EML
from django.conf import settings
from smtplib import SMTP
try:
    from email.mime.text import MIMEText
    from email.mime.multipart import MIMEMultipart
    from ecell.mime.image import MIMEImage
except:
    from email.MIMEText import MIMEText
    from email.MIMEMultipart import MIMEMultipart
    from email.MIMEImage import MIMEImage


# Create your views here.

def send_error_report_as_email(e, tb):
    import traceback
    currentLang = get_language()
    try:
        activate_lang('en_US')
        msg = MIMEText(''.join(traceback.format_exception(e.__class__, e, tb)))
        msg['From'] = 'feedback@chaperone.e-cell.org'
        msg['To'] = settings.FEEDBACK_RECIPIENT
        msg['Subject'] = _(u'E-Cell IDE Feedback Exception Report')
        conn = SMTP('localhost', 25, 'chaperone.e-cell.org')
        conn.sendmail('feedback@chaperone.e-cell.org', settings.FEEDBACK_RECIPIENT, msg.as_string())
    finally:
        activate_lang(currentLang)

def send_feedback_as_email(form):
    import codecs
    msg = MIMEMultipart()
    currentLang = get_language()
    try:
        activate_lang('en_US')
        msg['From'] = 'feedback@chaperone.e-cell.org'
        msg['To'] = settings.FEEDBACK_RECIPIENT
        msg['Subject'] = _(u'E-Cell IDE Feedback from %s') % form.cleaned_data['email']
        text = _(u'*** E-Cell IDE feedback ***\n')

        for field in form:
            rendered_text = ''
            data = form.cleaned_data[field.name]
            if isinstance(field.field, forms.FileField):
                if data != None:
                    rendered_text = "%s (%s)" % (
                        data.filename,
                        data.content_type
                        )
                else:
                    rendered_text = '(None)'
            elif isinstance(field.field, forms.ChoiceField):
                rendered_text = dict(field.field.choices)[data]
            else:
                rendered_text = data
            text += u"%s:\n%s\n\n" % ((field.label), rendered_text)
        msg.attach(MIMEText(
            codecs.getencoder('UTF-8')(text)[0],
            _charset = 'UTF-8'))
        if form.cleaned_data['screenshot'] != None:
            attach = MIMEImage(form.cleaned_data['screenshot'].read())
            msg.attach(attach)
        conn = SMTP('localhost', 25, 'chaperone.e-cell.org')
        conn.sendmail('feedback@chaperone.e-cell.org', settings.FEEDBACK_RECIPIENT, msg.as_string())
    finally:
        activate_lang(currentLang)

def show_form(request):
    vars = dict(form = FeedbackForm(), title = _(u'Feedback Form'))
    return render_to_response('feedbackform/form.html', vars,
        context_instance = RequestContext(request))

def thanks(request):
    vars = dict(title = _(u'Feedback Form'))
    return render_to_response('feedbackform/thanks.html', vars,
        context_instance = RequestContext(request))

def handle_form(request):
    if request.method == 'POST':
        form = FeedbackForm(request.POST, request.FILES)
        vars = dict(form = form)
        if form.is_valid():
            try:
                send_feedback_as_email(form)
                return HttpResponseRedirect(redirect_to = 'thanks')
            except Exception, e:
                try:
                    send_error_report_as_email(e, sys.exc_traceback)
                except e:
                    print e
                form._errors[NON_FIELD_ERRORS] = ErrorList([_(u'Failed to forward the feedback to the administrator. Please contact <info@e-cell.org> directly.')])
        return render_to_response('feedbackform/form.html', vars,
            context_instance = RequestContext(request))
    else:
        return show_form(request)

