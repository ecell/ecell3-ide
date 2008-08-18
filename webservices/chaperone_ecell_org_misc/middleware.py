import re
import os.path
import mimetypes
from django.http import HttpResponseNotFound, HttpResponseForbidden, HttpResponse

class DefaultMediaMiddleware(object):
    def __init__(self):
        from django.conf import settings
        self.media_dir = os.path.join(os.path.dirname(__file__), 'media')
        self.media_url = settings.MY_MEDIA_PREFIX

    def process_request(self, request):
        # Find the admin file and serve it up, if it exists and is readable.
        if not request.path.startswith(self.media_url):
            return None
        relative_url = re.sub(r'/+', '/', request.path)[len(self.media_url):]
        file_path = os.path.join(self.media_dir, relative_url)
        if not os.path.exists(file_path):
            resp = HttpResponseNotFound()
            resp.write("Not Found")
        else:
            try:
                fp = open(file_path, 'rb')
                resp = HttpResponse(
                    content_type = mimetypes.guess_type(file_path)[0])
                resp.write(fp.read())
                fp.close()
            except IOError:
                resp = HttpResponseForbidden()
                resp.write("Forbidden")
        return resp

