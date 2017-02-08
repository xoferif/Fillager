#!/bin/bash
echo deleting network configuration, and fetching a new one.
rm /etc/nginx/conf.d/default.conf
cp /srv/ngin/default.conf /etc/nginx/nginx.conf
echo All done
nginx -g "daemon off;"