#!/bin/bash
echo deleting network configuration
rm /etc/nginx/conf.d/default.conf
echo copying new configuration
cp /srv/ngin/default.conf /etc/nginx/nginx.conf
echo All done
nginx -g "daemon off;"