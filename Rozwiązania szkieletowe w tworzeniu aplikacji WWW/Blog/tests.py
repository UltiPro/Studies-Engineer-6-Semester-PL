from django.test import TestCase, SimpleTestCase, Client
from django.core.files.uploadedfile import SimpleUploadedFile
from django.urls import reverse, resolve
import tempfile

from .models import Newsletter as NewsletterModel, Author as AuthorModel, Tag as TagModel, Post as PostModel, User as UserModel, Comment as CommentModel
from .forms import NewsletterForm, UserRegisterForm, UserLoginForm, CommentForm, ChangeEmailForm, ChangePasswordForm, ChangeImageForm, DeleteAccountForm, ChangeDescriptionForm
from .views import Index as IndexView, Posts as PostsView, PostsBySearch as PostsBySearchView, PostsByTag as PostsByTagView, Post as PostView, Authors as AuthorsView, Author as AuthorView, AuthorPosts as AuthorPostsView, AuthorPostsByTag as AuthorPostsByTagView, About as AboutView, Register as RegisterView, Login as LoginView, Logout as LogoutView, Settings as SettingsView, User as UserView


class TestModels(TestCase):
    def setUp(self):
        self.newsletter = NewsletterModel.objects.create(
            name="Testname",
            surname="Testsurname",
            email="test@test.com"
        )
        self.author = AuthorModel.objects.create(
            name="Testname",
            surname="Testsurname",
            email="test@test.com"
        )
        self.tag = TagModel.objects.create(
            tag="Space"
        )
        self.post = PostModel.objects.create(
            title="Test title",
            author=self.author,
            content="Content test for post by any author",
            image="users/default.png"
        )
        self.post.tags.add(self.tag)
        self.user = UserModel.objects.create(
            login="test",
            password="login123456!",
            nickname="Test",
            email="Test@test.com"
        )
        self.comment = CommentModel.objects.create(
            post=self.post,
            user=self.user,
            content="Test content"
        )
        return super().setUp()

    def test_newsletter(self):
        self.assertEqual(NewsletterModel.objects.all().count(), 1)
        self.assertEquals(self.newsletter.__str__(), "Testname Testsurname")

    def test_author(self):
        self.assertEqual(AuthorModel.objects.all().count(), 1)
        self.assertEquals(self.author.__str__(), "Testname Testsurname")
        self.assertEquals(self.author.slug, "testname-testsurname")

    def test_tag(self):
        self.assertEqual(TagModel.objects.all().count(), 1)
        self.assertEquals(self.tag.tag, "Space")

    def test_post(self):
        self.assertEqual(PostModel.objects.all().count(), 1)
        self.assertEquals(self.post.__str__(),
                          "Test title by Testname Testsurname")
        self.assertEquals(self.post.slug, "test-title")

    def test_user(self):
        self.assertEqual(UserModel.objects.all().count(), 1)
        self.assertEquals(self.user.__str__(), "Test")
        self.assertEquals(self.user.slug, "test")

    def test_comment(self):
        self.assertEqual(CommentModel.objects.all().count(), 1)
        self.assertEquals(self.comment.__str__(), "Test - Test title")


class TestForms(TestCase):
    def setUp(self):
        self.client = Client()
        self.user = UserModel.objects.create(
            login="test",
            password="pbkdf2_sha256$390000$HJYDMzo8KCbbr0af49GcQu$lgA5Ocdb7pa2CA2WkP+r4g1JCYCYkCf97dvXbzWf+jM=",
            nickname="Test",
            email="test@test.com"
        )
        self.author = AuthorModel.objects.create(
            name="Testname",
            surname="Testsurname",
            email="test@test.com"
        )
        self.post = PostModel.objects.create(
            title="Test title",
            author=self.author,
            content="Content test for post by any author",
            image="users/default.png"
        )
        self.session = self.client.session
        self.session.update({
            "nickname": "Test"
        })
        self.session.save()
        return super().setUp()

    def test_newsletter_form_valid(self):
        form = NewsletterForm(data={
            "name": "Testname",
            "surname": "Testsurname",
            "email": "test@test.com"
        })
        self.assertTrue(form.is_valid())
        self.assertTrue(form.send_email())
        form.save()
        self.assertEqual(NewsletterModel.objects.all().count(), 1)

    def test_newsletter_form_invalid(self):
        form = NewsletterForm(data={
            "name": "",
            "surname": "",
            "email": "test@test."
        })
        self.assertFalse(form.is_valid())
        self.assertEquals(len(form.errors), 3)

    def test_user_registration_form_valid(self):
        form = UserRegisterForm(data={
            "login": "Test2",
            "password": "Test1234!",
            "c_password": "Test1234!",
            "nickname": "Test2",
            "email": "test2@test.com"
        })
        self.assertTrue(form.is_valid())
        form.save()
        self.assertEqual(UserModel.objects.all().count(), 2)

    def test_user_registration_form_invalid(self):
        form = UserRegisterForm(data={
            "login": "",
            "password": "Test123",
            "c_password": "Test1234",
            "nickname": "",
            "email": "test@test."
        })
        self.assertFalse(form.is_valid())
        self.assertEqual(len(form.errors), 5)

    def test_user_login_form_valid(self):
        form = UserLoginForm(data={
            "login": "Test",
            "password": "Test1234!"
        })
        self.assertTrue(form.is_valid())

    def test_user_login_form_invalid(self):
        form = UserLoginForm(data={
            "login": "",
            "password": "Test1234"
        })
        self.assertFalse(form.is_valid())
        self.assertEqual(len(form.errors), 2)

    def test_comment_form_valid(self):
        form = CommentForm(data={
            "content": "Test comment for any post"
        })
        self.assertTrue(form.is_valid())
        form.save(self.user.nickname, self.post.slug)
        self.assertEqual(CommentModel.objects.all().count(), 1)

    def test_comment_form_invalid(self):
        form = CommentForm(data={
            "content": ""
        })
        self.assertFalse(form.is_valid())
        self.assertEqual(len(form.errors), 1)

    def test_change_email_form_valid(self):
        form = ChangeEmailForm(data={
            "old_email": self.user.email,
            "email": "test3@test.com"
        }, instance=self.user)
        self.assertTrue(form.is_valid())
        form.save()
        self.assertEquals(UserModel.objects.get(
            nickname=self.user.nickname).email, "test3@test.com")

    def test_change_email_form_invalid(self):
        form = ChangeEmailForm(data={
            "old_email": self.user.email,
            "email": "test@test.com"
        }, instance=self.user)
        self.assertFalse(form.is_valid())
        self.assertEqual(len(form.errors), 1)

    def test_change_password_form_valid(self):
        form = ChangePasswordForm(data={
            "old_password": "Qwerty123456!",
            "password": "Test123456!"
        }, instance=self.user)
        self.assertTrue(form.is_valid())

    def test_change_password_form_invalid(self):
        form = ChangePasswordForm(data={
            "old_password": self.user.password,
            "password": "Test123"
        }, instance=self.user)
        self.assertFalse(form.is_valid())
        self.assertEqual(len(form.errors), 2)

    def test_change_image_form_valid(self):
        form = ChangeImageForm(data={
            "image": tempfile.NamedTemporaryFile(suffix=".jpg").name
        }, instance=self.user)
        self.assertTrue(form.is_valid())


class TestUrls(SimpleTestCase):
    def test_index(self):
        url = reverse("index")
        self.assertEquals(resolve(url).func.view_class, IndexView)

    def test_posts(self):
        url = reverse("posts")
        self.assertEquals(resolve(url).func.view_class, PostsView)

    def test_posts_by_search(self):
        url = reverse("posts_by_search")
        self.assertEquals(resolve(url).func.view_class, PostsBySearchView)

    def test_posts_by_tag(self):
        url = reverse("posts_by_tag", args=["slug"])
        self.assertEquals(resolve(url).func.view_class, PostsByTagView)

    def test_post(self):
        url = reverse("post", args=["slug"])
        self.assertEquals(resolve(url).func.view_class, PostView)

    def test_authors(self):
        url = reverse("authors")
        self.assertEquals(resolve(url).func.view_class, AuthorsView)

    def test_author(self):
        url = reverse("author", args=["slug"])
        self.assertEquals(resolve(url).func.view_class, AuthorView)

    def test_author_posts(self):
        url = reverse("author_posts", args=["slug"])
        self.assertEquals(resolve(url).func.view_class, AuthorPostsView)

    def test_author_posts_by_tag(self):
        url = reverse("author_posts_by_tag", args=["slug", "str"])
        self.assertEquals(resolve(url).func.view_class, AuthorPostsByTagView)

    def test_about_us(self):
        url = reverse("about_us")
        self.assertEquals(resolve(url).func.view_class, AboutView)

    def test_register(self):
        url = reverse("register")
        self.assertEquals(resolve(url).func.view_class, RegisterView)

    def test_login(self):
        url = reverse("login")
        self.assertEquals(resolve(url).func.view_class, LoginView)

    def test_logout(self):
        url = reverse("logout")
        self.assertEquals(resolve(url).func, LogoutView)

    def test_settings(self):
        url = reverse("settings")
        self.assertEquals(resolve(url).func.view_class, SettingsView)

    def test_user(self):
        url = reverse("user", args=["slug"])
        self.assertEquals(resolve(url).func.view_class, UserView)


class TestViews(TestCase):
    def setUp(self):
        self.client = Client()
        self.author = AuthorModel.objects.create(
            name="Testname",
            surname="Testsurname",
            email="test@test.com"
        )
        self.tag = TagModel.objects.create(
            tag="Space"
        )
        self.post = PostModel.objects.create(
            title="Test title",
            author=self.author,
            content="Content test for post by any author",
            image="users/default.png"
        )
        self.post.tags.add(self.tag)
        self.user = UserModel.objects.create(
            login="test",
            password="login123456!",
            nickname="Test",
            email="Test@test.com"
        )
        session = self.client.session
        session.update({
            "nickname": "Test",
            "image": "users/default.png"
        })
        session.save()
        return super().setUp()

    def test_index_get(self):
        response = self.client.get(reverse("index"))
        self.assertEquals(response.status_code, 200)
        self.assertTemplateUsed(response, "Blog/index.html")

    def test_index_post(self):
        response = self.client.post(reverse("index"), {
            "name": "Testname",
            "surname": "Testsurname",
            "email": "test@test.com"
        })
        self.assertEquals(response.status_code, 200)
        self.assertTrue(NewsletterModel.objects.get(email="test@test.com"))

    def test_index_post_empty(self):
        response = self.client.post(reverse("index"), {})
        self.assertEquals(response.status_code, 200)
        self.assertEqual(NewsletterModel.objects.all().count(), 0)

    def test_posts_get(self):
        response = self.client.get(reverse("posts"))
        self.assertEquals(response.status_code, 200)
        self.assertTemplateUsed(response, "Blog/posts.html")

    def test_posts_post(self):
        response = self.client.post(reverse("posts"), {
            "name": "Testname",
            "surname": "Testsurname",
            "email": "test2@test.com"
        })
        self.assertEquals(response.status_code, 200)
        self.assertTrue(NewsletterModel.objects.get(email="test2@test.com"))

    def test_posts_post_empty(self):
        response = self.client.post(reverse("posts"), {})
        self.assertEquals(response.status_code, 200)
        self.assertEqual(NewsletterModel.objects.all().count(), 0)

    def test_posts_by_search_get(self):
        response = self.client.get(
            reverse("posts_by_search"), {"search": self.post.title})
        self.assertEquals(response.status_code, 200)
        self.assertTemplateUsed(response, "Blog/posts_search.html")

    def test_posts_by_tag_get(self):
        response = self.client.get(reverse("posts_by_tag", args=[self.tag]))
        self.assertEquals(response.status_code, 200)
        self.assertTemplateUsed(response, "Blog/posts.html")

    def test_posts_by_tag_post(self):
        response = self.client.post(reverse("posts_by_tag", args=[self.tag]), {
            "name": "Testname",
            "surname": "Testsurname",
            "email": "test3@test.com"
        })
        self.assertEquals(response.status_code, 200)
        self.assertTrue(NewsletterModel.objects.get(email="test3@test.com"))

    def test_posts_by_tag_post_empty(self):
        response = self.client.post(
            reverse("posts_by_tag", args=[self.tag]), {})
        self.assertEquals(response.status_code, 200)
        self.assertEqual(NewsletterModel.objects.all().count(), 0)

    def test_post_get(self):
        response = self.client.get(reverse("post", args=[self.post.slug]))
        self.assertEquals(response.status_code, 200)
        self.assertTemplateUsed(response, "Blog/post.html")

    """def test_post_post(self):
        response = self.client.post(reverse("post", args=[self.post.slug]), {
            "content": "Test text for post method"
        })
        self.assertEquals(response.status_code, 200)
        self.assertTrue(CommentModel.objects.get(
            content="Test text for post method"))"""

    def test_authors_get(self):
        response = self.client.get(reverse("authors"))
        self.assertEquals(response.status_code, 200)
        self.assertTemplateUsed(response, "Blog/authors.html")

    def test_author_get(self):
        response = self.client.get(reverse("author", args=[self.author.slug]))
        self.assertEquals(response.status_code, 200)
        self.assertTemplateUsed(response, "Blog/author.html")

    def test_author_posts_get(self):
        response = self.client.get(
            reverse("author_posts", args=[self.author.slug]))
        self.assertEquals(response.status_code, 200)
        self.assertTemplateUsed(response, "Blog/author_posts.html")

    def test_author_posts_by_tag_get(self):
        response = self.client.get(
            reverse("author_posts_by_tag", args=[self.author.slug, self.tag.tag]))
        self.assertEquals(response.status_code, 200)
        self.assertTemplateUsed(response, "Blog/author_posts.html")

    def test_about_us_get(self):
        response = self.client.get(reverse("about_us"))
        self.assertEquals(response.status_code, 200)
        self.assertTemplateUsed(response, "Blog/about.html")

    def test_register_get(self):
        response = self.client.get(reverse("register"))
        self.assertEquals(response.status_code, 200)
        self.assertTemplateUsed(response, "Blog/register.html")

    def test_login_get(self):
        response = self.client.get(reverse("login"))
        self.assertEquals(response.status_code, 200)
        self.assertTemplateUsed(response, "Blog/login.html")

    def test_logout_get(self):
        response = self.client.get(reverse("logout"))
        self.assertEquals(response.status_code, 302)

    def test_settings_get(self):
        response = self.client.get(reverse("settings"))
        self.assertEquals(response.status_code, 200)
        self.assertTemplateUsed(response, "Blog/settings.html")

    def test_user_get(self):
        response = self.client.get(reverse("user", args=[self.user.slug]))
        self.assertEquals(response.status_code, 200)
        self.assertTemplateUsed(response, "Blog/user.html")
