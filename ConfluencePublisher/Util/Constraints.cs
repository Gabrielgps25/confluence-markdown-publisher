﻿namespace ConfluencePublisher
{
    internal class Constraints
    {
        public static readonly string SITE_SUBDIRECTORY = "confluence-site";

        public static readonly string CREATE_CONTENT = "/rest/api/content";
        public static readonly string UPDATE_CONTENT = "/rest/api/content/{0}";
        public static readonly string CREATE_ATTACHMENT = "/rest/api/content/{0}/child/attachment";
        public static readonly string UPDATE_ATTACHMENT = "/rest/api/content/{0}/child/attachment/{0}";


        public static readonly string GET_PAGE_PARAMETERS = "?spaceKey={0}&title={1}&expand=version";

    }
}