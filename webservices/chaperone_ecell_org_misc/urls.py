from django.conf.urls.defaults import *

# Uncomment the next two lines to enable the admin:
# from django.contrib import admin
# admin.autodiscover()

urlpatterns = patterns('',
    # Example:
    # (r'^sbml2eml/', include('sbml2eml.foo.urls')),

    # Uncomment the next line to enable admin documentation:
    # (r'^admin/doc/', include('django.contrib.admindocs.urls')),

    # Uncomment the next line for to enable the admin:
    # (r'^admin/(.*)', admin.site.root),

    (r'^services/sbml2eml/+$', 'chaperone_ecell_org_misc.sbml2eml.views.handle_form'),
    (r'^services/feedback/+$', 'chaperone_ecell_org_misc.feedbackform.views.handle_form'),
    (r'^services/feedback/thanks$', 'chaperone_ecell_org_misc.feedbackform.views.thanks')
)
