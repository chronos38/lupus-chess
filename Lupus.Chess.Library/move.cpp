#include "move.h"
#include "board.h"
#include "piece.h"
#include <string>

namespace chess {
    class move_castling : public move_state {
    public:
        move_castling(std::shared_ptr<board> board, castling castling, piece_color color, ipiece* king, ipiece* rook)
            : board_(board), castling_(castling), color_(color), king_(king), rook_(rook) {
            removed_.reserve(4);
        }

        virtual ~move_castling() = default;

        virtual void execute(move* move) final override {
            std::string castling = board_->castling();
            auto rank = color_ == white ? 1 : 8;
            auto king = color_ == white ? 'K' : 'k';
            auto queen = color_ == white ? 'Q' : 'q';

            switch (castling_) {
                case king_side: {
                    auto match = castling.find(king);

                    if (match != castling.npos) {
                        castling.erase(match, 1);
                        removed_ += king;
                        match = castling.find(queen);

                        if (match != castling.npos) {
                            castling.erase(match, 1);
                            removed_ += queen;
                        }

                        board_->set('e', rank, 0);
                        board_->set('h', rank, 0);
                        board_->set('g', rank, color_ == white ? white_king : black_king);
                        board_->set('f', rank, color_ == white ? white_rook : black_rook);
                        board_->set_castling(castling.c_str());
                        king_->set_position((std::string(1, 'g') + std::to_string(rank)).c_str());
                        rook_->set_position((std::string(1, 'f') + std::to_string(rank)).c_str());
                    }
                    break;
                }
                case queen_side: {
                    auto match = castling.find(king);

                    if (match != castling.npos) {
                        castling.erase(match, 1);
                        removed_ += queen;
                        match = castling.find(queen);

                        if (match != castling.npos) {
                            castling.erase(match, 1);
                            removed_ += king;
                        }

                        board_->set('e', rank, 0);
                        board_->set('a', rank, 0);
                        board_->set('c', rank, color_ == white ? white_king : black_king);
                        board_->set('d', rank, color_ == white ? white_rook : black_rook);
                        board_->set_castling(castling.c_str());
                        king_->set_position((std::string(1, 'c') + std::to_string(rank)).c_str());
                        rook_->set_position((std::string(1, 'd') + std::to_string(rank)).c_str());
                    }
                    break;
                }
                default:
                    break;
            }
        }

        virtual void undo(move* move) final override {
            auto rank = color_ == white ? 1 : 8;
            switch (castling_) {
                case king_side:
                    board_->set('g', rank, 0);
                    board_->set('f', rank, 0);
                    board_->set('e', rank, color_ == white ? white_king : black_king);
                    board_->set('h', rank, color_ == white ? white_rook : black_rook);
                    king_->set_position((std::string(1, 'e') + std::to_string(rank)).c_str());
                    rook_->set_position((std::string(1, 'h') + std::to_string(rank)).c_str());
                    break;
                case queen_side:
                    board_->set('c', rank, 0);
                    board_->set('d', rank, 0);
                    board_->set('e', rank, color_ == white ? white_king : black_king);
                    board_->set('a', rank, color_ == white ? white_rook : black_rook);
                    king_->set_position((std::string(1, 'e') + std::to_string(rank)).c_str());
                    rook_->set_position((std::string(1, 'a') + std::to_string(rank)).c_str());
                    break;
                default:
                    break;
            }

            board_->set_castling((board_->castling() + removed_).c_str());
            removed_.clear();
        }

        virtual std::string to_string(const move* move) const final override {
            switch (castling_) {
                case king_side:
                    return "O-O";
                case queen_side:
                    return "O-O-O";
                default:
                    return "";
            }
        }

        virtual const char* from() const final override {
            return "";
        }
        
        virtual const char* to() const final override {
            return "";
        }

        virtual void set_board(std::shared_ptr<board> value) final override {
            board_ = value;
        }

        virtual bool operator==(const move_state& other) const final override {
            auto o = dynamic_cast<const move_castling*>(std::addressof(other));
            return o && castling_ == o->castling_ && color_ == o->color_;
        }

        virtual bool operator!=(const move_state& other) const final override {
            return !(*this == other);
        }

    private:

        std::shared_ptr<board> board_;
        castling castling_ = no_side;
        piece_color color_ = white;
        std::string removed_;
        ipiece* king_ = nullptr;
        ipiece* rook_ = nullptr;
    };

    class move_piece : public move_state {
    public:
        move_piece(ipiece* piece, const char* from, const char* to) : piece_(piece) {
            auto from_length = strlen(from);
            auto to_length = strlen(to);
            from_.fill(0);
            to_.fill(0);

            if (from_length >= from_.max_size())
                from_length = from_.max_size() - 1;
            if (to_length >= to_.max_size())
                to_length = to_.max_size();

            std::copy_n(from, from_length, std::begin(from_));
            std::copy_n(to, to_length, std::begin(to_));
            board_ = piece->board();
        }

        virtual ~move_piece() = default;

        virtual void execute(move* move) override {
            auto piece = board_->get(from_.data());
            if (!piece)
                return;
            if (piece != piece_->value())
                return;
            auto to = board_->get(to_.data());
            if (to) {
                captured_ = to;
            }

            board_->set(from_.data(), 0);
            board_->set(to_.data(), piece);
            en_passant_ = board_->en_passant();
            board_->set_en_passant("-");
            piece_->set_position(to_.data());
        }

        virtual void undo(move* move) override {
            auto piece = board_->get(to_.data());
            if (!piece)
                return;
            if (piece != piece_->value())
                return;
            board_->set(to_.data(), captured_);
            board_->set(from_.data(), piece);
            if (captured_)
                captured_ = 0;
            board_->set_en_passant(en_passant_.c_str());
            en_passant_.clear();
            piece_->set_position(from_.data());
        }

        virtual std::string to_string(const move* move) const override {
            std::string result;
            result.reserve(16);

            if (piece_->type() != pawn)
                result += toupper(piece_->value());
            result += piece_->position()[0];

            if (captured_)
                result += 'x';
            result += to_.data();

            return result;
        }

        virtual const char* from() const final override {
            return from_.data();
        }
        
        virtual const char* to() const final override {
            return to_.data();
        }

        virtual void set_board(std::shared_ptr<board> value) final override {
            board_ = value;
        }

        virtual bool operator==(const move_state& other) const override {
            auto o = dynamic_cast<const move_piece*>(std::addressof(other));
            return o && piece_ == o->piece_ && from_ == o->from_ && to_ == o->to_ && captured_ == o->captured_;
        }

        virtual bool operator!=(const move_state& other) const override {
            return !(*this == other);
        }

    protected:

        ipiece* piece_ = nullptr;
        std::shared_ptr<board> board_;
        std::array<char, 3> from_;
        std::array<char, 3> to_;
        std::string en_passant_;
        uint8_t captured_ = 0;
    };

    class move_en_passant : public move_piece {
    public:
        move_en_passant(ipiece* piece, const char* from, const char* to) : move_piece(piece, from, to) {
            auto en_passant = board_->en_passant();
            file_ = en_passant[0];
            rank_ = atoi(&en_passant[1]);
        }

        virtual ~move_en_passant() = default;

        virtual void execute(move* move) override {
            move_piece::execute(move);

            switch (rank_) {
                case 3:
                    board_->set(file_, 4, 0);
                    captured_ = white_pawn;
                    break;
                case 6:
                    board_->set(file_, 5, 0);
                    captured_ = black_pawn;
                    break;
            }
        }

        virtual void undo(move* move) override {
            switch (rank_) {
                case 3:
                    board_->set(file_, 4, white_pawn);
                    captured_ = 0;
                    break;
                case 6:
                    board_->set(file_, 5, black_pawn);
                    captured_ = 0;
                    break;
            }

            move_piece::undo(move);
        }

        virtual std::string to_string(const move* move) const override {
            return move_piece::to_string(move) + "e.p.";
        }

    private:
        char file_ = 0;
        uint8_t rank_ = 0;
    };

    class move_pawn : public move_piece {
    public:
        move_pawn(ipiece* piece, const char* from, const char* to) : move_piece(piece, from, to) {}

        virtual ~move_pawn() = default;

        virtual void execute(move* move) override {
            move_piece::execute(move);
            auto rank = piece_->color() == white ? '2' : '7';
            auto promotion = piece_->color() == white ? '8' == to_[0] : '1' == to_[0];

            if (from_.at(1) == rank && abs(from_.at(1) - to_.at(1)) == 2)
                board_->set_en_passant((std::string(1, from_.at(0)) + std::string(1, piece_->color() == white ? '3' : '6')).c_str());
            if (promotion)
                board_->set(to_.data(), type_to_value(queen, piece_->color()));
        }

        virtual void undo(move* move) override {
            auto promotion = piece_->color() == white ? '8' == to_[0] : '1' == to_[0];
            board_->set(to_.data(), type_to_value(pawn, piece_->color()));
            move_piece::undo(move);
        }
    };

    class move_king : public move_piece {
    public:
        move_king(ipiece* piece, const char* from, const char* to) : move_piece(piece, from, to) {}

        virtual ~move_king() = default;

        virtual void execute(move* move) override {
            move_piece::execute(move);
            auto color = piece_->color();
            auto castling = castling_ = board_->castling();
            auto match = castling.find(color == white ? 'K' : 'k');
            if (match != castling.npos)
                castling.erase(match, 1);
            match = castling.find(color == white ? 'Q' : 'q');
            if (match != castling.npos)
                castling.erase(match, 1);
            board_->set_castling(castling.c_str());
        }

        virtual void undo(move* move) override {
            board_->set_castling(castling_.c_str());
            castling_.clear();
            move_piece::undo(move);
        }

    private:

        std::string castling_;
    };

    class move_rook : public move_piece {
    public:
        move_rook(ipiece* piece, const char* from, const char* to) : move_piece(piece, from, to) {}

        virtual ~move_rook() = default;

        virtual void execute(move* move) override {
            move_piece::execute(move);
            auto color = piece_->color();
            auto castling = castling_ = board_->castling();
            char side = 0;

            switch (color) {
                case white:
                    if (strcmp(from_.data(), "a1") == 0)
                        side = 'Q';
                    if (strcmp(from_.data(), "h1") == 0)
                        side = 'K';
                    break;
                case black:
                    if (strcmp(from_.data(), "a8") == 0)
                        side = 'q';
                    if (strcmp(from_.data(), "h8") == 0)
                        side = 'k';
                    break;
            }

            if (side) {
                auto match = castling.find(side);
                if (match != castling.npos)
                    castling.erase(match, 1);
                board_->set_castling(castling.c_str());
            }
        }

        virtual void undo(move* move) override {
            board_->set_castling(castling_.c_str());
            castling_.clear();
            move_piece::undo(move);
        }

    private:

        std::string castling_;
    };

    move::move(std::shared_ptr<board> board, castling castling, piece_color color, ipiece* king, ipiece* rook) {
        switch (castling) {
            case king_side:
            case queen_side:
                state_ = std::make_unique<move_castling>(board, castling, color, king, rook);
                break;
            default:
                throw std::invalid_argument("castling needs to be either king_side or queen_side");
        }
    }

    move::move(ipiece* piece, const char* from, const char* to) {
        if (!piece)
            throw std::invalid_argument("'piece' is null");
        if (!from)
            throw std::invalid_argument("'from' is null");
        if (!to)
            throw std::invalid_argument("'to' is null");
        auto from_length = strlen(from);
        auto to_length = strlen(to);

        if (from_length != 2)
            throw std::invalid_argument("'from' has to be exactly two characters long");
        if (to_length != 2)
            throw std::invalid_argument("'from' has to be exactly two characters long");
        if (from[0] < 'a' || from[0] > 'h')
            throw std::invalid_argument("'from' file value is invalid");
        if (from[1] < '1' || from[1] > '8')
            throw std::invalid_argument("'from' rank valud is invalid");
        if (to[0] < 'a' || to[0] > 'h')
            throw std::invalid_argument("'to' file value is invalid");
        if (to[1] < '1' || to[1] > '8')
            throw std::invalid_argument("'to' rank valud is invalid");
        if (piece->type() == pawn)
            if (strcmp(to, piece->board()->en_passant()) == 0)
                state_ = std::make_unique<move_en_passant>(piece, from, to);
            else
                state_ = std::make_unique<move_pawn>(piece, from, to);
        else if (piece->type() == king)
            state_ = std::make_unique<move_king>(piece, from, to);
        else if (piece->type() == rook)
            state_ = std::make_unique<move_rook>(piece, from, to);
        else
            state_ = std::make_unique<move_piece>(piece, from, to);
    }

    void move::execute() {
        state_->execute(this);
    }

    void move::undo() {
        state_->undo(this);
    }

    std::string move::to_string() const {
        return state_->to_string(this);
    }

    const char* move::from() const {
        return state_->from();
    }

    const char* move::to() const {
        return state_->to();
    }

    void move::set_board(std::shared_ptr<board> value) {
        state_->set_board(value);
    }

    bool move::operator==(const move& other) const {
        return *state_ == *other.state_;
    }

    bool move::operator!=(const move& other) const {
        return !(*this == other);
    }
}
