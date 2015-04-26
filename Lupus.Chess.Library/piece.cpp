#include "piece.h"
#include "board.h"
#include "move.h"
#include "movement.h"
#include <future>

namespace chess {
    const char* piece_position(std::shared_ptr<board> on_board, piece_value for_value, std::array<char, 3>& position) {
        auto file = 0;
        auto rank = 1;

        for (auto&& square : *on_board) {
            if (square == for_value) {
                position[0] = file + 'a';
                position[1] = rank + '0';
                return position.data();
            }

            file = (file + 1) % 8;
            if (file == 0)
                rank += 1;
        }

        return "";
    }

    class piece_queen : public piece_state {
    public:
        explicit piece_queen(piece_color color) : piece_state(color) {
            score_ = 1000;
        }

        virtual ~piece_queen() = default;
        
        virtual std::unique_ptr<piece_state> clone() const override {
            return std::make_unique<piece_queen>(color_);
        }

        virtual void update(ipiece* piece) override {
            collisions_.fill(0);

            auto left = std::async(std::launch::async, moves_till_end, piece->board(), direction::left, position_.data(), std::ref(collisions_[0]));
            auto right = std::async(std::launch::async, moves_till_end, piece->board(), direction::right, position_.data(), std::ref(collisions_[1]));
            auto up = std::async(std::launch::async, moves_till_end, piece->board(), direction::up, position_.data(), std::ref(collisions_[2]));
            auto down = std::async(std::launch::async, moves_till_end, piece->board(), direction::down, position_.data(), std::ref(collisions_[3]));
            auto lower_left = std::async(std::launch::async, moves_till_end, piece->board(), direction::lower_left, position_.data(), std::ref(collisions_[4]));
            auto lower_right = std::async(std::launch::async, moves_till_end, piece->board(), direction::lower_right, position_.data(), std::ref(collisions_[5]));
            auto upper_left = std::async(std::launch::async, moves_till_end, piece->board(), direction::upper_left, position_.data(), std::ref(collisions_[6]));
            auto upper_right = std::async(std::launch::async, moves_till_end, piece->board(), direction::upper_right, position_.data(), std::ref(collisions_[7]));
            piece_position(piece->board(), value(), position_);
        }
        
        virtual piece_value value() const override {
            switch (color_) {
                case white:
                    return white_queen;
                case black:
                    return black_queen;
                default:
                    throw std::domain_error("could not determine piece value");
            }
        }
        
        virtual piece_type type() const override {
            return queen;
        }
    };

    class piece_rook : public piece_state {
    public:
        explicit piece_rook(piece_color color) : piece_state(color) {
        }

        virtual ~piece_rook() = default;
        virtual std::unique_ptr<piece_state> clone() const override;
        virtual void update(ipiece* piece) override;
        virtual piece_value value() const override;
        virtual piece_type type() const override;
    };

    class piece_bishop : public piece_state {
    public:
        piece_bishop(piece_color color) : piece_state(color) {
        }

        virtual ~piece_bishop() = default;
        virtual std::unique_ptr<piece_state> clone() const override;
        virtual void update(ipiece* piece) override;
        virtual piece_value value() const override;
        virtual piece_type type() const override;
    };

    class piece_knight : public piece_state {
    public:
        piece_knight(piece_color color) : piece_state(color) {
        }

        virtual ~piece_knight() = default;
        virtual std::unique_ptr<piece_state> clone() const override;
        virtual void update(ipiece* piece) override;
        virtual piece_value value() const override;
        virtual piece_type type() const override;
    };

    class piece_pawn : public piece_state {
    public:
        piece_pawn() = default;

        piece_pawn(piece_color color) : piece_state(color) {
        }

        virtual ~piece_pawn() = default;
        virtual std::unique_ptr<piece_state> clone() const override;
        virtual void update(ipiece* piece) override;
        virtual piece_value value() const override;
        virtual piece_type type() const override;
    };

    class piece_pawn_white : public piece_pawn {
    public:
        virtual ~piece_pawn_white() = default;
        virtual std::unique_ptr<piece_state> clone() const override;
        virtual void update(ipiece* piece) override;
    };

    class piece_pawn_black : public piece_pawn {
    public:
        virtual ~piece_pawn_black() = default;
        virtual std::unique_ptr<piece_state> clone() const override;
        virtual void update(ipiece* piece) override;
    };

    class piece_king : public piece_state {
    public:
        piece_king() = default;

        piece_king(piece_color color) : piece_state(color) {
        }

        virtual ~piece_king() = default;
        virtual std::unique_ptr<piece_state> clone() const override;
        virtual void update(ipiece* piece) override;
        virtual piece_value value() const override;
        virtual piece_type type() const override;
    };

    class piece_king_white : public piece_king {
    public:
        virtual ~piece_king_white() = default;
        virtual std::unique_ptr<piece_state> clone() const override;
        virtual void update(ipiece* piece) override;
    };

    class piece_king_black : public piece_king {
    public:
        virtual ~piece_king_black() = default;
        virtual std::unique_ptr<piece_state> clone() const override;
        virtual void update(ipiece* piece) override;
    };

    class piece_king_end_game : public piece_king {
    public:
        piece_king_end_game(piece_color color);
        virtual ~piece_king_end_game() = default;
        virtual std::unique_ptr<piece_state> clone() const override;
        virtual void update(ipiece* piece) override;
    };

    piece::piece(std::shared_ptr<chess::board> board, piece_value value) 
        : piece(board, value_to_color(value), value_to_type(value)) {
    }

    piece::piece(std::shared_ptr<chess::board> board, piece_color color, piece_type type) {
        switch (type) {
            case pawn:
                switch (color) {
                    case white:
                        state_ = std::make_unique<piece_pawn_white>();
                        break;
                    case black:
                        state_ = std::make_unique<piece_pawn_black>();
                        break;
                }
                break;
            case knight:
                state_ = std::make_unique<piece_knight>(color);
                break;
            case bishop:
                state_ = std::make_unique<piece_bishop>(color);
                break;
            case rook:
                state_ = std::make_unique<piece_rook>(color);
                break;
            case queen:
                state_ = std::make_unique<piece_queen>(color);
                break;
            case king:
                if (board->count() <= 10) {
                    state_ = std::make_unique<piece_king_end_game>(color);
                } else {
                    switch (color) {
                        case white:
                            state_ = std::make_unique<piece_king_white>();
                            break;
                        case black:
                            state_ = std::make_unique<piece_king_black>();
                            break;
                    }
                }
                break;
        }
    }

    piece::piece(piece&& other) {
        swap(board_, other.board_);
        swap(state_, other.state_);
    }

    piece::piece(const piece& other) {
        board_ = other.board_;
        state_ = other.state_->clone();
    }

    void piece::update() {
        state_->update(this);
    }

    int piece::score() const {
        return state_->score();
    }

    int piece::attack_score() const {
        return state_->attack_score();
    }

    int piece::defense_score() const {
        return state_->defense_score();
    }

    int piece::position_score() const {
        return state_->position_score();
    }

    std::vector<std::shared_ptr<move>> piece::allowed_moves() const {
        return state_->allowed_moves();
    }

    piece_value piece::value() const {
        return state_->value();
    }

    piece_type piece::type() const {
        return state_->type();
    }

    piece_color piece::color() const {
        return state_->color();
    }

    const char* piece::position() const {
        return state_->position();
    }

    std::shared_ptr<board> piece::board() const {
        return board_;
    }

    piece& piece::operator=(piece&& other) {
        swap(board_, other.board_);
        swap(state_, other.state_);
        other.board_ = nullptr;
        other.state_ = nullptr;
        return *this;
    }

    piece& piece::operator=(const piece& other) {
        board_ = other.board_;
        state_ = other.state_->clone();
        return *this;
    }

    piece_state::piece_state(piece_color color) : color_(color) {
    }

    int piece_state::score() const {
        return score_;
    }

    int piece_state::attack_score() const {
        return attack_score_;
    }

    int piece_state::defense_score() const {
        return defense_score_;
    }

    int piece_state::position_score() const {
        return position_score_;
    }

    std::vector<std::shared_ptr<move>> piece_state::allowed_moves() const {
        return moves_;
    }

    const char* piece_state::position() const {
        return position_.data();
    }

    piece_color piece_state::color() const {
        return color_;
    }

    void piece_state::change_state(piece* piece, std::unique_ptr<piece_state> state) {
        piece->state_ = std::move(state);
    }
}
