#pragma once

namespace chess {
    class observer;

    class subject {
    public:
        virtual ~subject() = default;
        virtual void attach(observer* observer) = 0;
        virtual void detach(observer* observer) = 0;
        virtual void notify() = 0;
    };

    class observer {
    public:
        virtual ~observer() = default;
        virtual void notify(subject* subject) = 0;
    };
}
