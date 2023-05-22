# Generated by Django 4.1.7 on 2023-04-27 20:52

import django.core.validators
from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('Blog', '0013_alter_user_description'),
    ]

    operations = [
        migrations.AlterField(
            model_name='author',
            name='description',
            field=models.TextField(default='Information about author', validators=[django.core.validators.MinLengthValidator(30), django.core.validators.MaxLengthValidator(2000)]),
        ),
        migrations.AlterField(
            model_name='post',
            name='content',
            field=models.TextField(validators=[django.core.validators.MinLengthValidator(30), django.core.validators.MaxLengthValidator(10000)]),
        ),
        migrations.AlterField(
            model_name='user',
            name='description',
            field=models.TextField(default='Information about user', null=True, validators=[django.core.validators.MinLengthValidator(15), django.core.validators.MaxLengthValidator(2000)]),
        ),
        migrations.AlterField(
            model_name='user',
            name='password',
            field=models.CharField(max_length=100, validators=[django.core.validators.RegexValidator('^(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[.~!@#$%^&*()+=[\\]\\;:\'"/,|{}<>?])[a-zA-Z0-9.~!@#$%^&*()+=[\\]\\;:\'"/,|{}<>?]{8,40}$', message='Password must be between 8 and 40 characters long, contain one lowercase and one uppercase letter, one number and one special character.')]),
        ),
    ]
