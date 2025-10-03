// ��������� ����
document.addEventListener('DOMContentLoaded', function () {
    const menuToggle = document.querySelector('.menu-toggle');
    const navMenu = document.querySelector('.nav-menu');

    if (menuToggle && navMenu) {
        menuToggle.addEventListener('click', function () {
            const isExpanded = this.getAttribute('aria-expanded') === 'true';
            this.setAttribute('aria-expanded', !isExpanded);
            navMenu.classList.toggle('active');
        });
    }

    // ���������� ��������
    const filterButtons = document.querySelectorAll('.filter-btn');
    const campaignCards = document.querySelectorAll('.campaign-card');

    filterButtons.forEach(button => {
        button.addEventListener('click', function () {
            // ������� �������� ����� � ���� ������
            filterButtons.forEach(btn => btn.classList.remove('active'));
            // ��������� �������� ����� ������� ������
            this.classList.add('active');

            const filter = this.getAttribute('data-filter');

            campaignCards.forEach(card => {
                if (filter === 'all' || card.getAttribute('data-status') === filter) {
                    card.style.display = 'block';
                } else {
                    card.style.display = 'none';
                }
            });
        });
    });

    // ���������� ������� ��� �����������
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape' && navMenu.classList.contains('active')) {
            menuToggle.setAttribute('aria-expanded', 'false');
            navMenu.classList.remove('active');
            menuToggle.focus();
        }
    });
});