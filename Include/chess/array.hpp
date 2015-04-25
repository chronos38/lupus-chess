#pragma once
#include <array>

namespace chess {
    template <typename T, size_t WIDTH, size_t HEIGHT>
    class array_2d {
    public:
        template <typename U>
        class index_adapter {
        public:
            explicit index_adapter(size_t row, U& ref) : row_(row), ref_(std::addressof(ref)) {}

            explicit index_adapter(size_t row, const U& ref) : row_(row), const_ref_(std::addressof(ref)) {}

            T& operator[](size_t column) {
                if (ref_)
                    return ref_->at(row_ * WIDTH + column);
                throw std::domain_error("ref_ is null");
            }

            const T& operator[](size_t column) const {
                if (const_ref_)
                    return const_ref_->at(row_ * WIDTH + column);
                if (ref_)
                    return ref_->at(row_ * WIDTH + column);
                throw std::domain_error("ref_ and const_ref_ is null");
            }

        private:
            size_t row_ = 0;
            U* ref_ = nullptr;
            const U* const_ref_ = nullptr;
        };

        array_2d() {
            array_.fill(T());
        }

        array_2d(array_2d&& other) {
            std::swap(array_, other.array_);
            other.array_.fill(T());
        }

        array_2d(const array_2d& other) {
            array_ = other.array_;
        }

        virtual ~array_2d() = default;

        typename std::array<T, WIDTH * HEIGHT>::iterator begin() {
            return std::begin(array_);
        }

        typename std::array<T, WIDTH * HEIGHT>::const_iterator begin() const {
            return std::begin(array_);
        }

        typename std::array<T, WIDTH * HEIGHT>::iterator end() {
            return std::end(array_);
        }

        typename std::array<T, WIDTH * HEIGHT>::const_iterator end() const {
            return std::end(array_);
        }

        const T& get(size_t index) const {
            return array_.at(index);
        }

        void set(size_t index, const T& value) {
            array_.at(index) = value;
        }

        const T& get(size_t row, size_t column) const {
            return array_.at(row * WIDTH + column);
        }

        void set(size_t row, size_t column, const T& value) {
            array_.at(row * WIDTH + column) = value;
        }

        index_adapter<std::array<T, WIDTH * HEIGHT>> operator[](size_t row) {
            return index_adapter<std::array<T, WIDTH * HEIGHT>>(row, array_);
        }

        const index_adapter<std::array<T, WIDTH * HEIGHT>> operator[](size_t row) const {
            return index_adapter<std::array<T, WIDTH * HEIGHT>>(row, array_);
        }

        array_2d& operator=(array_2d<T, WIDTH, HEIGHT>&& other) {
            std::swap(array_, other.array_);
            other.array_.fill(T());
            return *this;
        }

        array_2d& operator=(const array_2d<T, WIDTH, HEIGHT>& other) {
            array_ = other.array_;
            return *this;
        }
    private:
        std::array<T, WIDTH * HEIGHT> array_;
    };
}
